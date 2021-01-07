using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Snail.Common;
using Snail.Common.Extenssions;
using Snail.Core;
using Snail.RS.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace Snail.RS
{
    public class RSService
    {

        private DbContext db;
        private ILogger _logger;
        private IMapper _mapper;
        public RSService(DbContext db, ILogger<RSService> logger, IMapper mapper)
        {
            this.db = db;
            _logger = logger;
            _mapper = mapper;
        }

     

        #region 规则
        /// <summary>
        /// 保存排班规则 
        /// </summary>
        /// <param name="rSScheduleRuleDto"></param>
        public void SaveRule(RSScheduleRuleDto rSScheduleRuleDto)
        {
            var validResult = new List<ValidationResult>();
            if (!Validator.TryValidateObject(rSScheduleRuleDto, new ValidationContext(rSScheduleRuleDto), validResult))
            {
                throw new BusinessException(string.Join(",", validResult.Select(a => a.ErrorMessage)));
            }
            if (!rSScheduleRuleDto.Id.HasValue())
            {
                rSScheduleRuleDto.Id = IdGenerator.Generate<string>();
                db.Set<RSScheduleRule>().Add(_mapper.Map<RSScheduleRule>(rSScheduleRuleDto));
            }
            else
            {
                var entity = db.Set<RSScheduleRule>().FirstOrDefault(a => a.Id == rSScheduleRuleDto.Id);
                if (entity != null)
                {
                    _mapper.Map<RSScheduleRuleDto, RSScheduleRule>(rSScheduleRuleDto, entity);
                }
            }
            db.SaveChanges();
        }

        public IQueryable<RSScheduleRule> QueryAllRule()
        {
            return db.Set<RSScheduleRule>().AsNoTracking();
        }

        public void DeleteRule(List<string> ids)
        {
            ids.ForEach(id =>
            {
                var entity = db.Set<RSScheduleRule>().FirstOrDefault(a => a.Id == id);
                if (entity == null)
                {
                    return;
                }
                if (db.Set<RSRecord>().Any(a => db.Set<RSScheduleOfDay>().Where(i => i.RuleId == id).Select(i => i.Id).Contains(a.ScheduleOfDayId)))
                {
                    entity.IsDeleted = true;
                    entity.UpdateTime = DateTime.Now;
                }
                else
                {
                    db.Set<RSScheduleRule>().Remove(entity);
                }

            });
            db.SaveChanges();
        }


        /// <summary>
        /// 生成排班
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        public void GenerateSchedule(DateTime beginDate, DateTime endDate)
        {
            var existsSchedule = db.Set<RSScheduleOfDay>().Where(a => a.ScheduleDate.Date >= beginDate.Date && a.ScheduleDate.Date <= endDate.Date).Select(a => new
            {
                a.RuleId,
                a.ScheduleDate,
                a.ScheduleName,
            }).ToList();
            var rules = db.Set<RSScheduleRule>().Where(a => !a.IsDeleted).AsNoTracking().ToList();
            var schedules = new List<RSScheduleOfDay>();
            foreach (var rule in rules)
            {
                schedules.AddRange(GenerateScheduleByRule(rule, beginDate, endDate));
            }
            foreach (var schedule in schedules)
            {
                if (!existsSchedule.Any(a => a.RuleId == schedule.RuleId && a.ScheduleDate.Date == schedule.ScheduleDate.Date && a.ScheduleName == schedule.ScheduleName))
                {
                    db.Set<RSScheduleOfDay>().Add(schedule);
                }
            }
            db.SaveChanges();
        }

        /// <summary>
        /// 重新生成一段时间内的预约号源
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        public void GenerateNewScheduleRemoveOld(DateTime beginDate, DateTime endDate)
        {
            var hasRecord = db.Set<RSRecord>().Where(a => a.ScheduleDate.Date >= beginDate.Date && a.ScheduleDate.Date <= endDate.Date).Select(a => a.ScheduleOfDayId).Distinct();
            var olds = db.Set<RSScheduleOfDay>().Where(a => !hasRecord.Contains(a.Id)).ToList();
            foreach (var old in olds)
            {
                db.Set<RSScheduleOfDay>().Remove(old);
            }
            db.SaveChanges();//要保存，不然在GenerateSchedule还是能找到删除的
            GenerateSchedule(beginDate, endDate);
        }
        #endregion


        #region 排班
        /// <summary>
        /// 获取排班
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public IQueryable<RSScheduleOfDayDto> GetScheduleOfDayQuery()
        {
            var query = from rule in db.Set<RSScheduleRule>().Where(a => !a.IsDeleted)
                        join schedule in db.Set<RSScheduleOfDay>().Where(a => !a.IsDeleted) on rule.Id equals schedule.RuleId
                        select new RSScheduleOfDayDto
                        {
                            RuleId = rule.Id,
                            Id = schedule.Id,
                            ExtraInfo = schedule.ExtraInfo,
                            RemainNum = schedule.RemainNum,
                            RemainNums = schedule.RemainNums,
                            ScheduleDate = schedule.ScheduleDate,
                            ScheduleName = rule.ScheduleName,
                            TargetId = rule.TargetId,
                            TargetName = rule.TargetName,
                            TargetType = rule.TargetType,
                            MaxNum=rule.MaxNum,
                            RuleBeginTime=rule.BeginTime,
                            RuleEndTime=rule.EndTime
                        };
            return query.AsNoTracking().OrderBy(a=>a.ScheduleDate).ThenBy(a=>a.RuleBeginTime);
        }
        #endregion


        /// <summary>
        /// 预约
        /// </summary>
        /// <param name="makeAppointDto"></param>
        /// <returns></returns>
        public RSRecordDto MakeAppoint(MakeAppointDto makeAppointDto)
        {
            var scheduleOfDay = db.Set<RSScheduleOfDay>().FirstOrDefault(a => a.Id == makeAppointDto.ScheduleOfDayId);
            if (scheduleOfDay == null)
            {
                throw new BusinessException("找不到排班");
            }
            var rule = db.Set<RSScheduleRule>().FirstOrDefault(a => a.Id == scheduleOfDay.RuleId);
            if (rule == null)
            {
                throw new BusinessException("找不到规则");
            }
            var occupy = Occupy(scheduleOfDay.RemainNums, makeAppointDto.OrderNum);
            scheduleOfDay.RemainNum = occupy.remainNum;
            scheduleOfDay.RemainNums = occupy.remainNums;
            var numBeginEndTime = GetNumBeginEndTime(rule.BeginTime, rule.EndTime, rule.MaxNum, occupy.orderNum);
            var record = new RSRecord
            {
                Id = IdGenerator.Generate<string>(),
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                IsDeleted = false,
                OrderNum = occupy.orderNum,
                ScheduleOfDayId = scheduleOfDay.Id,
                ExtraInfo = makeAppointDto.ExtraInfo,
                SubscriberId = makeAppointDto.SubscriberId,
                SubscriberName = makeAppointDto.SubscriberName,
                SubscriberPhone = makeAppointDto.SubscriberPhone,
                NumBeginTime = numBeginEndTime.begin,
                NumEndTime = numBeginEndTime.end,
                ScheduleDate= scheduleOfDay.ScheduleDate
            };
            db.Set<RSRecord>().Add(record);
            db.SaveChanges();
            var dto = _mapper.Map<RSRecordDto>(record);
            dto.TargetName = rule.TargetName;
            dto.ScheduleName = rule.ScheduleName;
            dto.RuleBeginTime = rule.BeginTime;
            dto.RuleEndTime = rule.EndTime;
            return dto;
        }

        /// <summary>
        /// 释放号源/取消预约
        /// </summary>
        /// <param name="rsRecordId"></param>
        public void ReleaseAppoint(string rsRecordId)
        {
            var record = db.Set<RSRecord>().FirstOrDefault(a => a.Id == rsRecordId);
            var schedule = db.Set<RSScheduleOfDay>().FirstOrDefault(a => a.Id == record.ScheduleOfDayId);
            var release = Release(schedule.RemainNums, record.OrderNum);
            schedule.RemainNum = release.RemainNum;
            schedule.RemainNums = release.RemainNums;
            db.Set<RSRecord>().Remove(record);
            db.SaveChanges();
        }

        public IQueryable<RSRecordDto> GetRecordQuery()
        {
            var query = from a in db.Set<RSRecord>()
                        join b in db.Set< RSScheduleOfDay>() on a.ScheduleOfDayId equals b.Id into b_group
                        from b in b_group.DefaultIfEmpty()
                        join c in db.Set<RSScheduleRule>() on b.RuleId equals c.Id into c_group
                        from c in c_group.DefaultIfEmpty()
                        select new RSRecordDto
                        {
                            Id=a.Id,
                            OrderNum = a.OrderNum,
                            ScheduleName = b.ScheduleName,
                            ScheduleOfDayId = a.ScheduleOfDayId,
                            ExtraInfo = a.ExtraInfo,
                            NumBeginTime=a.NumBeginTime,
                            NumEndTime=a.NumEndTime,
                            RuleBeginTime=c.BeginTime,
                            RuleEndTime=c.EndTime,
                            SubscriberId=a.SubscriberId,
                            SubscriberName=a.SubscriberName,
                            SubscriberPhone=a.SubscriberPhone,
                            TargetName=c.TargetName,
                            ScheduleDate=a.ScheduleDate
                        };
            return query;
        }


        #region 帮助类方法
        public static List<RSScheduleOfDay> GenerateScheduleByRule(RSScheduleRule rule, DateTime beginDate, DateTime endDate)
        {
            var result = new List<RSScheduleOfDay>();
            var dates = GetRuleDate(rule, beginDate, endDate);
            foreach (var date in dates)
            {
                result.Add(new RSScheduleOfDay
                {
                    Id = IdGenerator.Generate<string>(),
                    CreateTime = DateTime.Now,
                    IsDeleted = false,
                    RemainNum = rule.MaxNum,
                    RemainNums = GetRemainNumsByMaxNum(rule.MaxNum),
                    ScheduleDate = date,
                    ScheduleName = rule.ScheduleName,
                    UpdateTime = DateTime.Now,
                    RuleId = rule.Id,
                    ExtraInfo = rule.ExtraInfo
                });
            }
            return result;
        }

        public static string GetRemainNumsByMaxNum(int maxNum)
        {
            var remainNums = new List<string>();
            for (int i = 0; i < maxNum; i++)
            {
                remainNums.Add((i + 1).ToString());
            }
            return string.Join(",", remainNums);
        }
        public static List<DateTime> GetRuleDate(RSScheduleRule rule, DateTime beginDate, DateTime endDate)
        {
            beginDate = beginDate.Date;
            endDate = endDate.Date;
            var result = new List<DateTime>();
            var days = (endDate - beginDate).Days;
            if (days < 0)
            {
                return result;
            }
            for (int i = 0; i < days; i++)
            {
                var thisDate = beginDate.AddDays(i);
                if (rule.ExceptDate?.Split(',').Any(a => a == thisDate.ToString("yyyyMMdd")) ?? false)
                {
                    continue;
                }
                if (rule.InDate?.Split(',').Any(a => a == thisDate.ToString("yyyyMMdd")) ?? false)
                {
                    result.Add(thisDate);
                    continue;
                }
                if (rule.DayType == EScheduleDayType.Month
                    && (rule.DayList?.Split(',').Any(a => a == thisDate.Day.ToString()) ?? false))
                {
                    result.Add(thisDate);
                    continue;
                }
                if (rule.DayType == EScheduleDayType.Week
                    && (rule.DayList?.Split(',').Any(a => a == GetDayOfWeekInt(thisDate.DayOfWeek).ToString()) ?? false))
                {
                    result.Add(thisDate);
                    continue;
                }
            }
            return result;
        }

        public static int GetDayOfWeekInt(DayOfWeek dayOfWeek)
        {
            return dayOfWeek == DayOfWeek.Sunday ? 7 : (int)dayOfWeek;
        }

        /// <summary>
        /// 占号
        /// </summary>
        /// <param name="remainNums">剩余号</param>
        /// <param name="occupyNum">要预约哪个固定号，没有时就顺预约</param>
        /// <returns>remainNum：剩余多少号，remainNums：剩余号，orderNum：预约到哪个号</returns>
        public static (int remainNum, string remainNums, int orderNum) Occupy(string remainNums, int occupyNum)
        {
            var remainList = remainNums.Split(new char[] { ','}, StringSplitOptions.RemoveEmptyEntries).OrderBy(a => int.Parse(a)).ToList();
            if (!remainList.Any())
            {
                throw new BusinessException("余号不足");
            }
            var orderNum = "";
            if (occupyNum > 0)
            {
                if (remainList.Contains(occupyNum.ToString()))
                {
                    orderNum = occupyNum.ToString();
                }
                else
                {
                    throw new BusinessException($"{occupyNum}号已经被预约");
                }
            }
            else
            {
                orderNum = remainList.FirstOrDefault();
            }
            remainList.Remove(orderNum);
            var remainNumsTemp = string.Join(",", remainList);
            return (remainList.Count, remainNumsTemp, int.Parse(orderNum));
        }
        public static (int RemainNum, string RemainNums) Release(string remainNums, int orderNum)
        {
            var remainList = remainNums.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            remainList.Add(orderNum.ToString());
            remainList = remainList.OrderBy(a => int.Parse(a)).ToList();
            var remainNumsTemp = string.Join(",", remainList);
            return (remainList.Count, remainNumsTemp);
        }

        /// <summary>
        /// 获取号的预计开始结束时间
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="totalNum"></param>
        /// <param name="orderNum"></param>
        /// <returns></returns>
        public static (TimeSpan begin, TimeSpan end) GetNumBeginEndTime(TimeSpan begin, TimeSpan end, int totalNum, int orderNum)
        {
            try
            {
                totalNum = totalNum == 0 ? 1 : totalNum;
                var minPerNum = (int)((end - begin).TotalMinutes / totalNum);
                var orderEndTime = begin + new TimeSpan(0, minPerNum * orderNum, 0);
                var orderBeginTime = orderEndTime - new TimeSpan(0, minPerNum, 0);
                return (orderBeginTime, orderEndTime);
            }
            catch (Exception ex)
            {
                return (TimeSpan.Zero, TimeSpan.Zero);
            }
        }
        public static void DealWithRSScheduleOfDayDto(RSScheduleOfDayDto dto)
        {
            dto.RemainNumsDto = new List<RemainNumDto>();
            (dto.RemainNums ?? "").Split(',').Select(a=>int.Parse(a)).OrderBy(a=>a).ToList().ForEach(num =>
            {
                var beginEnd = RSService.GetNumBeginEndTime(dto.RuleBeginTime, dto.RuleEndTime, dto.MaxNum, num);
                dto.RemainNumsDto.Add(new RemainNumDto { Num = num, TimeBegin = beginEnd.begin, TimeEnd = beginEnd.end });
            });
        }
        #endregion



    }
}
