using System;

namespace TaskManager.Web.Extensions {
    public static class HtmlHelper1 {
        public static string GetHtml(this string text) {
            return text.Replace(Environment.NewLine, "<br/>");
        }
    }
}
