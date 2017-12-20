using System;

namespace TaskManager.Web.Models {
    public class WebApiResult {
        public bool success { get; private set; }
        public string message { get; private set; }
        public string error { get; private set; }
        public object data { get; private set; }

        private WebApiResult(string error) {
            this.error = error;
            this.success = false;
        }

        private WebApiResult(object data) : this() {
            this.data = data;
        }

        private WebApiResult(object data, string message) : this() {
            this.data = data;
            this.message = message;
        }

        private WebApiResult() {
            this.success = true;
            this.message = "Success!";
        }

        public static WebApiResult Failed(string error) {
            return new WebApiResult(error);
        }

        public static WebApiResult Failed(Exception exception) {
            return new WebApiResult(exception.GetBaseException().Message);
        }

        public static WebApiResult Succeed(object data) {
            return new WebApiResult(data);
        }

        public static WebApiResult Succeed() {
            return new WebApiResult();
        }
    }
}