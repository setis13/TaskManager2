using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace System.Web.Http {
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ValidateAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter {
        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation) {
            try {
                var formToken = actionContext.Request.Headers
                    .GetValues("__RequestVerificationToken")
                    .FirstOrDefault();
                var cookieToken = actionContext.Request.Headers
                    .GetCookies()
                    .SelectMany(e => e.Cookies)
                    .FirstOrDefault(e => e.Name == "__RequestVerificationToken")
                    ?.Value;
                if (formToken != null && cookieToken != null) {
                    AntiForgery.Validate(cookieToken, formToken);
                }
            } catch {
                actionContext.Response = new HttpResponseMessage {
                    StatusCode = HttpStatusCode.Forbidden,
                    RequestMessage = actionContext.ControllerContext.Request
                };
                return FromResult(actionContext.Response);
            }
            return continuation();
        }

        private Task<HttpResponseMessage> FromResult(HttpResponseMessage result) {
            var source = new TaskCompletionSource<HttpResponseMessage>();
            source.SetResult(result);
            return source.Task;
        }
    }
}