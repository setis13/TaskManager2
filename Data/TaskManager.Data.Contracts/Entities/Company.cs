using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TaskManager.Data.Identity;

namespace TaskManager.Data.Entities {
    public class Company : BaseEntity {
        [DataType("VARCHAR"), MaxLength(64), Required]
        public string Name { get; set; }

        public ICollection<TaskManagerUser> Users { get; set; }
    }
}
