using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using LmsBackend.Authorization;

namespace LmsBackend
{
    [DependsOn(
        typeof(LmsBackendCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class LmsBackendApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<LmsBackendAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(LmsBackendApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
