using System;
using System.Collections.Generic;
using System.Text;

namespace Snail.Common
{
    /**
     * 使用一个 64 bit 的 long 型的数字作为全局唯一 id
     * 这 64 个 bit 中，其中 第1 个 bit 是不用的，然后用其中的 41 bit 作为毫秒数，用 10 bit 作为工作机器 id，12 bit 作为序列号。
     * 第1个bit:为定值0，如果第一位为1就是负数，但id是正数，所以第一位为0
     * 后41bit:为毫秒数
     * 后10bit：其中前5位为机房即最大为32，后5位是机器即最大数为32
     * 后12bit:这个是用来记录同一个毫秒内同一个机房的同一个机子产生的不同 id
     * 
     * 
     * 
     * **/
    public class SnowflakeId
    {
        public const long Twepoch = 1577836800000L;//开始时间截 ,2020/1/1 0:00:00 +00:00

        private const int WorkerIdBits = 5;//机器id所占的位数
        private const int DatacenterIdBits = 5;//机房id所占的位数
        private const int SequenceBits = 12;//序列在id中占的位数
        private const long MaxWorkerId = -1L ^ (-1L << WorkerIdBits);//最大机器数,为31
        private const long MaxDatacenterId = -1L ^ (-1L << DatacenterIdBits);//最大机房数,为31

        private const int WorkerIdShift = SequenceBits;//机器id向左移12位
        private const int DatacenterIdShift = SequenceBits + WorkerIdBits;////机房id向左移12+5位
        public const int TimestampLeftShift = SequenceBits + WorkerIdBits + DatacenterIdBits;//毫秒数向左移12+5+5位
        private const long SequenceMask = -1L ^ (-1L << SequenceBits);//序列最大数,为4095

        private static SnowflakeId _snowflakeId;// 定义全局静态snowflake对象

        private readonly object _lock = new object();
        private static readonly object SLock = new object();
        private long _lastTimestamp = -1L;

        public SnowflakeId(long workerId, long datacenterId, long sequence = 0L)
        {
            WorkerId = workerId;
            DatacenterId = datacenterId;
            Sequence = sequence;

            // sanity check for workerId
            if (workerId > MaxWorkerId || workerId < 0)
                throw new ArgumentException($"worker Id can't be greater than {MaxWorkerId} or less than 0");

            if (datacenterId > MaxDatacenterId || datacenterId < 0)
                throw new ArgumentException($"datacenter Id can't be greater than {MaxDatacenterId} or less than 0");
        }

        public long WorkerId { get; protected set; }
        public long DatacenterId { get; protected set; }

        public long Sequence { get; internal set; }

        public static long Create()
        {
            if (_snowflakeId==null)
            {
                Default();
            }
            return _snowflakeId.NextId();
        }

        public static SnowflakeId Default()
        {
            lock (SLock)
            {
                if (_snowflakeId != null)
                {
                    return _snowflakeId;
                }

                var random = new Random();

                if (!int.TryParse(Environment.GetEnvironmentVariable("SNOWFLAKE_WORKERID", EnvironmentVariableTarget.Machine), out var workerId))
                {
                    workerId = random.Next((int)MaxWorkerId);
                }

                if (!int.TryParse(Environment.GetEnvironmentVariable("SNOWFLAKE_DATACENTERID", EnvironmentVariableTarget.Machine), out var datacenterId))
                {
                    datacenterId = random.Next((int)MaxDatacenterId);
                }

                return _snowflakeId = new SnowflakeId(workerId, datacenterId);
            }
        }

        /// <summary>
        /// 生成id
        /// </summary>
        /// <returns></returns>
        public virtual long NextId()
        {
            lock (_lock)
            {
                var timestamp = TimeGen();

                //如果当前时间小于上一次ID生成的时间戳，说明系统时钟回退过这个时候应当抛出异常
                if (timestamp < _lastTimestamp)
                    throw new Exception(
                        $"InvalidSystemClock: Clock moved backwards, Refusing to generate id for {_lastTimestamp - timestamp} milliseconds");

                //如果是同一时间生成的，则进行毫秒内序列
                if (_lastTimestamp == timestamp)
                {
                    Sequence = (Sequence + 1) & SequenceMask;
                    if (Sequence == 0) timestamp = TilNextMillis(_lastTimestamp);//毫秒内序列溢出,阻塞到下一个毫秒,获得新的时间戳
                }
                else
                {
                    //时间戳改变，毫秒内序列重置
                    Sequence = 0;
                }

                _lastTimestamp = timestamp;// 更新"上次生成ID的时间截"的值

                // 移位并通过或运算拼到一起组成64位的ID
                var id = ((timestamp - Twepoch) << TimestampLeftShift) |
                         (DatacenterId << DatacenterIdShift) |
                         (WorkerId << WorkerIdShift) | Sequence;

                return id;
            }
        }

        /// <summary>
        /// 阻塞到下一个毫秒，直到获得新的时间戳
        /// </summary>
        /// <param name="lastTimestamp">上次生成ID的时间截</param>
        /// <returns>当前时间戳</returns>
        protected virtual long TilNextMillis(long lastTimestamp)
        {
            var timestamp = TimeGen();
            while (timestamp <= lastTimestamp) timestamp = TimeGen();
            return timestamp;
        }

        protected virtual long TimeGen()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }
}
