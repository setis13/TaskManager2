using System;

namespace TaskManager.Logic.Dtos {

    public class SessionDto : BaseDto {
        public DateTime LastActivity { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
    }
}
