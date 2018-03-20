using System.Linq;
using System.Net.Http.Headers;

namespace TaskManager.Data.Contracts.Helpers {
    public static class HttpHeadersHelper {
        public static string GetHeader(HttpRequestHeaders headers, string key) {
            if (headers.Contains(key)) {
                return headers.GetValues(key).First();
            } else {
                return null;
            }
        }
    }
}
