using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Common.Identity;
using TaskManager.Data.Contracts.Entities.Base;

namespace TaskManager.Data.Contracts.Entities {
    public class Company : BaseEntity {
        [DataType("VARCHAR"), MaxLength(64), Required]
        public string Name { get; set; }

        public ICollection<TaskManagerUser> Users { get; set; }
    }
}
