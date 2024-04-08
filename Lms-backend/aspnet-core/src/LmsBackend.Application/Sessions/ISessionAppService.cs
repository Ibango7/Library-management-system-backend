using System.Threading.Tasks;
using Abp.Application.Services;
using LmsBackend.Sessions.Dto;

namespace LmsBackend.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
