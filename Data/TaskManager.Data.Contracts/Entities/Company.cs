using System.ComponentModel.DataAnnotations;
using TaskManager.Data.Contracts.Entities.Base;

namespace TaskManager.Data.Contracts.Entities {
    public class Company : BaseEntity {
        [DataType("VARCHAR"), MaxLength(64), Required]
        public string Name { get; set; }
    }
}
