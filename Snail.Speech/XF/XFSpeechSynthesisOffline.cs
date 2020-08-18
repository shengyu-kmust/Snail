using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Options;
namespace Snail.Speech.XF
{
    /// <summary>
    /// 讯飞离线语音合成
    /// </summary>
    public class XFSpeechSynthesisOffline : ISpeechSynthesis
    {
        private IOptionsMonitor<XFSpeechOption> _optionsMonitor;
        public XFSpeechSynthesisOffline(IOptionsMonitor<XFSpeechOption> optionsMonitor)
        {
            _optionsMonitor = optionsMonitor;
        }
        public byte[] Synthesis(string content, EAudioType eAudioType)
        {
            ValidateOption();
            byte[] result;
            var ret = 0;
            var session_ID =new IntPtr();
            var option = _optionsMonitor.CurrentValue;
            try
            {
                ///APPID请勿随意改动
                string login_configs = $"appid ={option.AppId} ";//登录参数,自己注册后获取的appid
                if (string.IsNullOrEmpty(content))
                {
                    throw new ArgumentException("请输入合成语音的内容");
                }

                uint audio_len = 0;

                SynthStatus synth_status = SynthStatus.MSP_TTS_FLAG_STILL_HAVE_DATA;
                ret = XFTtsHelper.MSPLogin(string.Empty, string.Empty, login_configs);//第一个参数为用户名，第二个参数为密码，第三个参数是登录参数，用户名和密码需要在http://open.voicecloud.cn
                                                                                      //MSPLogin方法返回失败
                if (ret != (int)ErrorCode.MSP_SUCCESS)
                {
                    throw new ArgumentException($"登录失败，失败error为{ret}");
                }
                //string _params = "engine_type = local, voice_name=xiaoyan, tts_res_path =fo|res\\tts\\xiaoyan.jet;fo|res\\tts\\common.jet, sample_rate = 16000";
                //string _params = "ssm=1,ent=sms16k,vcn=xiaoyan,spd=medium,aue=speex-wb;7,vol=x-loud,auf=audio/L16;rate=16000";

                session_ID = XFTtsHelper.QTTSSessionBegin(option.SessionBeginParams, ref ret);
                //session_ID = XFTtsHelper.QTTSSessionBegin(_params, ref ret);
                //QTTSSessionBegin方法返回失败
                if (ret != (int)ErrorCode.MSP_SUCCESS)
                {
                    throw new ArgumentException($"QTTSSessionBegin方法返回失败：{ret}");
                }
                ret = XFTtsHelper.QTTSTextPut(Ptr2Str(session_ID), content, (uint)Encoding.Default.GetByteCount(content), string.Empty);
                //QTTSTextPut方法返回失败
                if (ret != (int)ErrorCode.MSP_SUCCESS)
                {
                    throw new ArgumentException($"QTTSTextPut方法返回失败：{ret}");
                }
                using (var memoryStream = new MemoryStream())
                {
                    memoryStream.Write(new byte[44], 0, 44);
                    while (true)
                    {
                        IntPtr source = XFTtsHelper.QTTSAudioGet(Ptr2Str(session_ID), ref audio_len, ref synth_status, ref ret);
                        if (ret!=0)
                        {
                            break;
                        }
                        byte[] array = new byte[(int)audio_len];
                        if (audio_len > 0 && source!=default)
                        {
                            Marshal.Copy(source, array, 0, (int)audio_len); 
                            memoryStream.Write(array, 0, array.Length);
                        }
                        //Thread.Sleep(10);//官方文档是建议等待一段时间，但经测试，太长了。先去除
                        if (synth_status == SynthStatus.MSP_TTS_FLAG_DATA_END || ret != 0)
                            break;
                    }
                    WAVE_Header wave_Header = getWave_Header((int)memoryStream.Length - 44);
                    byte[] array2 = StructToBytes(wave_Header);
                    memoryStream.Position = 0L;
                    memoryStream.Write(array2, 0, array2.Length);
                    memoryStream.Position = 0L;
                    result = memoryStream.ToArray();
                }
                if (eAudioType!=EAudioType.WAV)
                {
                    throw new Exception("未实现WAV转MP3");
                }
                return result; 
            }
            finally
            {
                XFTtsHelper.QTTSSessionEnd(Ptr2Str(session_ID), "");
                XFTtsHelper.MSPLogout();//退出登录
            }
        }

        private void ValidateOption()
        {
            if (string.IsNullOrEmpty(_optionsMonitor.CurrentValue.AppId))
            {
                throw new Exception("讯飞语音的AppId参数未配置");

            }
            if (string.IsNullOrEmpty(_optionsMonitor.CurrentValue.SessionBeginParams))
            {
                throw new Exception("讯飞语音的SessionBeginParams参数未配置");
            }
        }

        /// <summary>
        /// 结构体转字符串
        /// </summary>
        /// <param name="structure"></param>
        /// <returns></returns>
        private static byte[] StructToBytes(object structure)
        {
            int num = Marshal.SizeOf(structure);
            IntPtr intPtr = Marshal.AllocHGlobal(num);
            byte[] result;
            try
            {
                Marshal.StructureToPtr(structure, intPtr, false);
                byte[] array = new byte[num];
                Marshal.Copy(intPtr, array, 0, num);
                result = array;
            }
            finally
            {
                Marshal.FreeHGlobal(intPtr);
            }
            return result;
        }

        /// <summary>
        /// 结构体初始化赋值
        /// </summary>
        /// <param name="data_len"></param>
        /// <returns></returns>
        private static WAVE_Header getWave_Header(int data_len)
        {
            return new WAVE_Header
            {
                RIFF_ID = 1179011410,
                File_Size = data_len + 36,
                RIFF_Type = 1163280727,
                FMT_ID = 544501094,
                FMT_Size = 16,
                FMT_Tag = 1,
                FMT_Channel = 1,
                FMT_SamplesPerSec = 16000,
                AvgBytesPerSec = 32000,
                BlockAlign = 2,
                BitsPerSample = 16,
                DATA_ID = 1635017060,
                DATA_Size = data_len
            };
        }
        /// <summary>
        /// 语音音频头
        /// </summary>
        private struct WAVE_Header
        {
            public int RIFF_ID;
            public int File_Size;
            public int RIFF_Type;
            public int FMT_ID;
            public int FMT_Size;
            public short FMT_Tag;
            public ushort FMT_Channel;
            public int FMT_SamplesPerSec;
            public int AvgBytesPerSec;
            public ushort BlockAlign;
            public ushort BitsPerSample;
            public int DATA_ID;
            public int DATA_Size;
        }
        /// 指针转字符串
        /// </summary>
        /// <param name="p">指向非托管代码字符串的指针</param>
        /// <returns>返回指针指向的字符串</returns>
        public static string Ptr2Str(IntPtr p)
        {
            return Marshal.PtrToStringAnsi(p);//下面这一段和这一句的效果是一样的。
            //List<byte> lb = new List<byte>();
            //while (Marshal.ReadByte(p) != 0)
            //{
            //    lb.Add(Marshal.ReadByte(p));
            //    p = p + 1;
            //}
            //byte[] bs = lb.ToArray();
            //var result = Encoding.Default.GetString(lb.ToArray());
            //return result;
        }
    }
}
