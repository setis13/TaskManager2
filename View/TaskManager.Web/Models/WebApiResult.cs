using System;

namespace TaskManager.Web.Models {
    public class WebApiResult {
        public bool Success { get; private set; }
        public string Message { get; private set; }
        public object Data { get; private set; }

        private WebApiResult(string error) {
            this.Message = error;
            this.Success = false;
        }

        private WebApiResult(object data) : this() {
            this.Data = data;
        }

        private WebApiResult(object data, string message) : this() {
            this.Data = data;
            this.Message = message;
        }

        private WebApiResult() {
            this.Success = true;
            this.Message = "Success!";
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