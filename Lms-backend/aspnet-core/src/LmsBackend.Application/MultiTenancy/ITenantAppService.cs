using Abp.Application.Services;
using LmsBackend.MultiTenancy.Dto;

namespace LmsBackend.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

