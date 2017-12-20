using System.ComponentModel.DataAnnotations;
using TaskManager.Data.Contracts.Entities.Base;

namespace TaskManager.Data.Contracts.Entities {
    /// <summary>
    ///     Session entity </summary>
    /// <remarks> Represent Entity of database </remarks>
    public class Session : BaseEntity {
        /// <summary>
        ///     Last session activity </summary>
        [Required]
        public System.DateTime LastActivity { get; set; }
        /// <summary>
        ///     Client IP address </summary>
        [Required]
        [MaxLength(30)]
        public string IpAddress { get; set; }
        /// <summary>
        ///     Client User Agent string </summary>
        [Required]
        [MaxLength(512)]
        public string UserAgent { get; set; }
    }
}