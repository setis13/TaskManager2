using System;
using TaskManager.Logic.Contracts.Dtos.Base;

namespace TaskManager.Logic.Contracts.Dtos {

    public class SessionDto : BaseDto {
        public DateTime LastActivity { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
    }
}
