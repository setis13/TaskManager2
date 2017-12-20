using Microsoft.Owin;
using Owin;
using TaskManager.Web;

[assembly: OwinStartup(typeof(Startup))]

namespace TaskManager.Web {
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
