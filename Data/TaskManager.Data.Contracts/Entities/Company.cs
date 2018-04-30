using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TaskManager.Data.Contracts.Entities.Base;
using TaskManager.Data.Contracts.Identity;

namespace TaskManager.Data.Contracts.Entities {
    public class Company : BaseEntity {
        [DataType("VARCHAR"), MaxLength(64), Required]
        public string Name { get; set; }

        public ICollection<TaskManagerUser> Users { get; set; }
    }
}
