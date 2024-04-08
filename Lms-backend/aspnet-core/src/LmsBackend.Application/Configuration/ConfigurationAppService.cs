using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using LmsBackend.Configuration.Dto;

namespace LmsBackend.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : LmsBackendAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
