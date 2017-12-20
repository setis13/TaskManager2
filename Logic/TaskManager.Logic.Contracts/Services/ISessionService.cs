using TaskManager.Logic.Contracts.Dtos;
using TaskManager.Logic.Contracts.Services.Base;

namespace TaskManager.Logic.Contracts.Services {
    /// <summary>
    ///     The Session Service interface. </summary>
    public interface ISessionService : IEntityReadonlyService<SessionDto> {
        /// <summary>
        ///     Authenticates the session or creates a new session if one does not exist. </summary>
        /// <returns>The <see cref="SessionDto" />.</returns>
        SessionDto Authenticate();

        /// <summary>
        ///     The set cookie expiry. </summary>
        /// <param name="minutes">The minutes.</param>
        void SetCookieExpiry(int minutes);

        /// <summary>
        ///     Deletes the users cookie. </summary>
        void DeleteCookie();

        /// <summary>
        ///     The get session id from cookie. </summary>
        string GetSessionIdFromCookie();

        /// <summary>
        ///     Clears old sessions </summary>
        void DeleteOldSessions();
    }
}