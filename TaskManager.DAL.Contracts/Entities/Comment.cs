using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DAL.Contracts.Entities.Base;

namespace TaskManager.DAL.Contracts.Entities {
    public class Comment : BaseEntity {
        public string Text { get; set; }
        public TimeSpan? Hours { get; set; }
        public Guid TaskId { get; set; }

        [ForeignKey("TaskId")]
        public Entities.Task Task { get; set; }
    }
}
