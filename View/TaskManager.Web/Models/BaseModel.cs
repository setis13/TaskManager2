using TaskManager.Logic.Contracts.Dtos;

namespace TaskManager.Web.Models {
    public class BaseModel {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseModel" /> class. </summary>
        /// <param name="session">The session.</param>
        public BaseModel(SessionDto session) {
            this.CurrentSession = session;
        }

        /// <summary>
        ///     Gets or sets the current session. </summary>
        public SessionDto CurrentSession { get; set; }
    }
}