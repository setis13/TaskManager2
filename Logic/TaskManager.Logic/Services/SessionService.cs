using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using AutoMapper;
using TaskManager.Common;
using TaskManager.Data;
using TaskManager.Data.Entities;
using TaskManager.Data.Extensions;
using TaskManager.Data.Identity;
using TaskManager.Logic.Dtos;

namespace TaskManager.Logic.Services {
    /// <summary>
    ///     The session service. </summary>
    public class SessionService : HostService<ISessionService>, ISessionService {

        #region [ Static ]

        #endregion [ Static ]

        #region [ Constants ]

        /// <summary>
        ///     Session cookie Name </summary>
        private const string CookieName = "Session";

        #endregion [ Constants ]

        #region [ .ctor ]

        /// <summary>
        ///     Initializes a new instance of the <see cref="SessionService" /> class. </summary>
        public SessionService(IServicesHost servicesHost, IUnitOfWork unitOfWork, IMapper mapper)
            : base(servicesHost, unitOfWork, mapper) {
        }

        #endregion [ .ctor ]

        #region [ Public ]

        /// <summary>
        ///     The authenticate. </summary>
        /// <returns>The <see cref="SessionDto" />.</returns>
        public SessionDto Authenticate() {
            var httpContext = HttpContext.Current;
            try {
                var domain = this.GetRootDomain(httpContext.Request.Url.Host);
                var authCookie = this.SelectAuthenticationCookie(domain, httpContext);

                if (authCookie == null) {
                    return this.CreateNewSession(httpContext, domain);
                }

                // User already has an authentication cookie.
                Guid sessionId;
                if (Guid.TryParse(authCookie.Value, out sessionId)) {
                    var tempSession = this.UnitOfWork.GetRepository<Session>().GetById(sessionId);
                    if (tempSession != null) {
                        var userId = httpContext.User.Identity.IsAuthenticated
                             ? httpContext.User.Identity.GetUserId()
                             : TaskManagerUser.SystemAdmin.Id;
                        tempSession.LastActivity = DateTime.UtcNow;
                        this.UnitOfWork.GetRepository<Session>().Update(tempSession, userId);
                        this.UnitOfWork.SaveChanges();
                        return this.Mapper.Map<SessionDto>(tempSession);
                    }
                    return this.CreateNewSession(httpContext, domain);
                }
                return this.CreateNewSession(httpContext, domain);
            } catch (Exception ex) {
                Logger.f("Error Getting Session", ex, "SecurityHelper");
                return this.CreateNewSession(httpContext, this.GetRootDomain(httpContext.Request.Url.Host));
            }
        }

        /// <summary>
        ///     The get by id. </summary>
        /// <param name="id">The id.</param>
        /// <returns>The <see cref="SessionDto" />.</returns>
        public SessionDto GetById(Guid id) {
            var session = this.UnitOfWork.GetRepository<Session>().GetById(id);
            return this.Mapper.Map<SessionDto>(session);
        }

        /// <summary>
        ///     Searches entities with predicate </summary>
        /// <param name="predicate">Predicate</param>
        /// <returns>List of entities</returns>
        public List<SessionDto> SearchFor<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : BaseEntity {
            var store = this.UnitOfWork.GetRepository<TEntity>().SearchFor(predicate);
            return this.Mapper.Map<List<SessionDto>>(store);
        }

        /// <summary>
        ///     Get all sessions list </summary>
        /// <returns>Sessions</returns>
        public List<SessionDto> GetAll() {
            var session = this.UnitOfWork.GetRepository<Session>().GetAll();
            return this.Mapper.Map<List<SessionDto>>(session);
        }

        /// <summary>
        ///     The get session id from cookie. </summary>
        /// <returns>The <see cref="string" />.</returns>
        public string GetSessionIdFromCookie() {
            var domain = this.GetRootDomain(HttpContext.Current.Request.Url.Host);

            var authCookie = this.SelectAuthenticationCookie(domain, null);
            if (authCookie == null) {
                return null;
            }

            return authCookie.Value;
        }

        /// <summary>
        ///     The set cookie expiry. </summary>
        /// <param name="minutes">The minutes.</param>
        public void SetCookieExpiry(int minutes) {
            var domain = this.GetRootDomain(HttpContext.Current.Request.Url.Host);

            var authCookie = this.SelectAuthenticationCookie(domain, null);
            authCookie.Domain = domain;
            authCookie.Expires = minutes != 0 ? DateTime.UtcNow.AddMinutes(minutes) : DateTime.MinValue;

            HttpContext.Current.Response.Cookies.Set(authCookie);
        }

        /// <summary>
        ///     Delete's the users cookie. </summary>
        public void DeleteCookie() {
            var domain = this.GetRootDomain(HttpContext.Current.Request.Url.Host);
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(CookieName + "." + domain, string.Empty) {
                Domain = domain
            });
        }

        /// <summary>
        ///     TODO: Clears old sessions </summary>
        public void DeleteOldSessions() {
            //UnitOfWork.Context.ExecuteSqlCommand("DELETE FROM [dbo].[Session] WHERE EntityId IN (SELECT TOP 50000 EntityId FROM [dbo].[Session] WHERE DATEDIFF(DAY, [LastActivity], GETDATE()) >= 1)");
        }

        #endregion [ Public ]

        #region [ private ]

        /// <summary>
        ///     The create new session. </summary>
        /// <param name="context">The context.</param>
        /// <param name="domain">The domain.</param>
        /// <returns>The <see cref="SessionDto" />.</returns>
        private SessionDto CreateNewSession(HttpContext context, string domain) {

            var sessionCode = Guid.NewGuid();
            this.SetAuthenticationCookie(context, domain, sessionCode.ToString(), false,
                /*TODO*/(int)TimeSpan.FromDays(30 * 12).TotalMinutes);

            var userId = context.User.Identity.IsAuthenticated
                ? context.User.Identity.GetUserId()
                : TaskManagerUser.SystemAdmin.Id;

            var session = new Session {
                EntityId = sessionCode,
                IpAddress = context.Request.UserHostAddress,
                UserAgent = context.Request.UserAgent ?? "UNKNOWN",
                LastActivity = DateTime.UtcNow,
                
            };

            // Add to the database.
            this.UnitOfWork.GetRepository<Session>().Insert(session, userId);
            this.UnitOfWork.SaveChanges();

            return this.Mapper.Map<SessionDto>(session);
        }

        /// <summary>
        ///     The get full cookie name. </summary>
        /// <param name="domain">The domain.</param>
        /// <returns>The <see cref="string" />.</returns>
        private string GetFullCookieName(string domain) {
            var authCookieName = new StringBuilder(CookieName);
            authCookieName.Append(".");
            authCookieName.Append(domain);
            return authCookieName.ToString();
        }

        /// <summary>
        ///     The get root domain. </summary>
        /// <param name="rootDomain">The root domain.</param>
        /// <returns>The <see cref="string" />.</returns>
        private string GetRootDomain(string rootDomain) {
            if (!rootDomain.StartsWith("192.168.")) {
                if (!string.IsNullOrEmpty(rootDomain)) {
                    var parts = rootDomain.Split('.');

                    if (parts.Length > 2) {
                        rootDomain = string.Join(".", parts, 1, parts.Length - 1);
                    }

                    if (rootDomain.EndsWith("/")) {
                        rootDomain = rootDomain.Substring(0, rootDomain.Length - 1);
                    }
                }
            }

            return rootDomain;
        }

        /// <summary>
        ///     The select authentication cookie. </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="httpContext">The http context.</param>
        /// <returns>The <see cref="HttpCookie" />.</returns>
        private HttpCookie SelectAuthenticationCookie(string domain, HttpContext httpContext) {
            HttpCookie authCookie = null;
            if (httpContext != null) {
                for (var index = 0; index < httpContext.Request.Cookies.Count; index++) {
                    var cookie = httpContext.Request.Cookies[index];
                    if (cookie.Name == this.GetFullCookieName(domain)) {
                        authCookie = cookie;
                        break;
                    }
                }
            } else {
                for (var index = 0; index < HttpContext.Current.Request.Cookies.Count; index++) {
                    var cookie = HttpContext.Current.Request.Cookies[index];
                    if (cookie.Name == this.GetFullCookieName(domain)) {
                        authCookie = cookie;
                        break;
                    }
                }
            }

            return authCookie;
        }

        /// <summary>
        ///     The set session cookie. </summary>
        /// <param name="httpContext">The http context.</param>
        /// <param name="domain">The domain.</param>
        /// <param name="email">The email.</param>
        /// <param name="persist">The persist.</param>
        /// <param name="minutes">The minutes.</param>
        private void SetAuthenticationCookie(HttpContext httpContext, string domain, string email, bool persist,
            int minutes) {
            // TODO - Encrypt this
            var cookie = new HttpCookie(this.GetFullCookieName(domain), email) {
                Domain = domain,
                Expires = DateTime.UtcNow.AddMinutes(minutes)
            };

            httpContext.Response.Cookies.Add(cookie);
        }

        #endregion [ private ]
    }
}