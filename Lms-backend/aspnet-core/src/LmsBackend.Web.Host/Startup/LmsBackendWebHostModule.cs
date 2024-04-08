using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using LmsBackend.Configuration;

namespace LmsBackend.Web.Host.Startup
{
    [DependsOn(
       typeof(LmsBackendWebCoreModule))]
    public class LmsBackendWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public LmsBackendWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LmsBackendWebHostModule).GetAssembly());
        }
    }
}
