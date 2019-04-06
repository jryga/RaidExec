using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RaidExec.Startup))]
namespace RaidExec
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
