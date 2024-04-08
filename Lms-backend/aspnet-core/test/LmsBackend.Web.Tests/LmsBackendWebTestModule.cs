using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using LmsBackend.EntityFrameworkCore;
using LmsBackend.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace LmsBackend.Web.Tests
{
    [DependsOn(
        typeof(LmsBackendWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class LmsBackendWebTestModule : AbpModule
    {
        public LmsBackendWebTestModule(LmsBackendEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LmsBackendWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(LmsBackendWebMvcModule).Assembly);
        }
    }
}