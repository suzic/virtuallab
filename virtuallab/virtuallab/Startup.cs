using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(virtuallab.Startup))]
namespace virtuallab
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
