using AutoMapper;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Data.Contracts.Entities.Base;

namespace TaskManager.Data.Contracts.Entities {
    public class Alarm : BaseEntity {
        [DataType("VARCHAR"), MaxLength(64), Required]
        public string Title { get; set; }
        [DataType("VARCHAR"), MaxLength(256)]
        public string Description { get; set; }
        [Column(TypeName = "Date")]
        public DateTime Date { get; set; }

        public byte? RepeatType { get; set; }
        public byte? RepeatValue { get; set; }
        public bool Birthday { get; set; }
        public bool Holiday { get; set; }
    }
}
