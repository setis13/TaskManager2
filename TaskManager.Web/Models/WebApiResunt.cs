using System;

namespace TaskManager.Web.Models {
    public class WebApiResunt {
        public bool success { get; private set; }
        public string message { get; private set; }
        public string error { get; private set; }
        public object data { get; private set; }

        private WebApiResunt(string error) {
            this.error = error;
            this.success = false;
        }

        private WebApiResunt(object data) : this() {
            this.data = data;
        }

        private WebApiResunt(object data, string message) : this() {
            this.data = data;
            this.message = message;
        }

        private WebApiResunt() {
            this.success = true;
            this.message = "Success!";
        }

        public static WebApiResunt Failed(string error) {
            return new WebApiResunt(error);
        }

        public static WebApiResunt Failed(Exception exception) {
            return new WebApiResunt(exception.GetBaseException().Message);
        }

        public static WebApiResunt Succeed(object data) {
            return new WebApiResunt(data);
        }

        public static WebApiResunt Succeed() {
            return new WebApiResunt();
        }
    }
}