using System;

namespace TaskManager.Logic.Contracts.Dtos {
    public class UserDto {
        public Guid Id { get; set; }
        /// <summary>
        ///     Company ID </summary>
        /// <remarks> I want to have company for every user because I don't use Nullable[Guid] </remarks>
        public Guid CompanyId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
