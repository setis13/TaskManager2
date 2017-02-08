using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DAL.Contracts.Entities.Base;

namespace TaskManager.DAL.Contracts.Entities {
    public class Project : BaseEntity {
        public string Name { get; set; }
    }
}
