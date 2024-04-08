using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace LmsBackend.Controllers
{
    public abstract class LmsBackendControllerBase: AbpController
    {
        protected LmsBackendControllerBase()
        {
            LocalizationSourceName = LmsBackendConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
