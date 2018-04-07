using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Personal_TrainerService.Startup))]

namespace Personal_TrainerService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}