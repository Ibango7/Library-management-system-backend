using System.Threading.Tasks;
using LmsBackend.Configuration.Dto;

namespace LmsBackend.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
