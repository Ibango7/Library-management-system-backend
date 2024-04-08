using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using LmsBackend.EntityFrameworkCore.Seed;


namespace LmsBackend.EntityFrameworkCore
{
    [DependsOn(
        typeof(LmsBackendCoreModule), 
        typeof(AbpZeroCoreEntityFrameworkCoreModule))]
    public class LmsBackendEntityFrameworkModule : AbpModule
    {
        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
            {
                Configuration.Modules.AbpEfCore().AddDbContext<LmsBackendDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                    {
                        LmsBackendDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        LmsBackendDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                    }
                });
            }
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LmsBackendEntityFrameworkModule).GetAssembly());
        
        }

        public override void PostInitialize()
        {
            if (!SkipDbSeed)
            {
                SeedHelper.SeedHostDb(IocManager);
            }


        }


    }
}
