using System;

namespace TaskManager.Logic.Contracts.Dtos {
    public class UserDto {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
