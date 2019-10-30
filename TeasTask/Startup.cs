using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TeasTask.Startup))]
namespace TeasTask
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
