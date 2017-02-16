using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TaskManager.Web.Startup))]
namespace TaskManager.Web {
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
        }
    }
}
