using Net5SerilogEFCore3D.Model.DomainCoreModels;
using System;

namespace Net5SerilogEFCore3D.Model.DomainModels
{
    public  class Serilog: IntEntity
    {
        protected Serilog() { }
        public Serilog(int id,  string message, string level, DateTimeOffset timeStamp,string exception, string logEvent
           , string remarks, bool? isDeleted, bool? isEnable, DateTimeOffset? createTime, Guid? createUserId, Guid? modifyUserId)
        {
            Id = id;
            Message = message;
            Level = level;
            TimeStamp = timeStamp;
            Exception = exception;
            LogEvent = logEvent;
            
            
            Remarks = remarks;
            IsDeleted = isDeleted;
            IsEnable = isEnable;
            CreateTime = createTime;
            CreateUserId = createUserId;
            ModifyUserId = modifyUserId;
        }

        public string Message { get; private set; }
        public string Level { get; private set; }
        public DateTimeOffset TimeStamp { get; private set; }
        public string Exception { get; private set; }
        public string LogEvent { get; private set; }
    }
}
