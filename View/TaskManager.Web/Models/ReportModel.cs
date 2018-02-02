using System;
using System.Collections.Generic;

namespace TaskManager.Web.Models {
    public class ReportModel {
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public List<Guid> ProjectIds { get; set; }
    }
}