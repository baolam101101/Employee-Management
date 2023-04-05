using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Resit_Project.Startup))]
namespace Resit_Project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
