using System;
namespace TaskManager.Data {
    /// <summary>
    ///     User Session Interface. Registered unity type </summary>
    public interface IUserSession {
        /// <summary>
        ///     Current User ID </summary>
        Guid UserId { get; set; }
        /// <summary>
        ///     Token </summary>
        string Token { get; set; }
        /// <summary>
        ///     Token Expires </summary>
        DateTime TokenExpires { get; set; }
    }
}
