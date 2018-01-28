using System;

namespace TaskManager.Logic.Contracts.Dtos {
    public class UserDto {
        public Guid Id { get; set; }
        /// <summary>
        ///     Company ID </summary>
        /// <remarks> User can create a object only with company ID </remarks>
        public Guid CompanyId { get; set; }
        public Guid? InvitationCompanyId { get; set; }
        public byte SortBy { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
