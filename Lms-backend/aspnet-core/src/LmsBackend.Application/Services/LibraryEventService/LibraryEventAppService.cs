using Abp.Application.Services;
using Abp.Domain.Repositories;
using LmsBackend.Services.LibraryEventService.Dto;
using LmsBackend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;

namespace LmsBackend.Services.LibraryEventService
{
    [AbpAuthorize]
    public class LibraryEventAppService: AsyncCrudAppService<LibraryEvent, LibraryEventDto, Guid>
    {
        public LibraryEventAppService(IRepository<LibraryEvent, Guid> repository) : base(repository) { }
    }
}
