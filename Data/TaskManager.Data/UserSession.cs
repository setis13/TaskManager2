using System;
using TaskManager.Data.Contracts;

namespace TaskManager.Data {
    /// <summary>
    ///     User Session </summary>
    public class UserSession : IUserSession {
        /// <summary>
        ///     Current User ID </summary>
       public Guid UserId { get; set; }
        /// <summary>
        ///     Token </summary>
        public string Token { get; set; }
        /// <summary>
        ///     Token Expires </summary>
        public DateTime TokenExpires { get; set; }
        /// <summary>
        ///     DTO of log rules </summary>
        public object LogRules { get; set; }
        /// <summary>
        ///     DTO of carrier info </summary>
        public object CarrierInfo { get; set; }
        /// <summary>
        ///     DTO of user </summary>
        public object User { get; set; }

        public void Reset() {
            throw new NotImplementedException();
        }

        public void Set(IUserSession session) {
            throw new NotImplementedException();
        }

        public object Clone() {
            var clone = new UserSession();
            clone.UserId = this.UserId;
            clone.Token = this.Token;
            clone.TokenExpires = this.TokenExpires;
            clone.LogRules = ((ICloneable)this.LogRules)?.Clone();
            clone.CarrierInfo = ((ICloneable)this.CarrierInfo)?.Clone();
            clone.User = ((ICloneable)this.User)?.Clone();

            return clone;
        }
    }
}