using Abp.Application.Services;
using Abp.Domain.Repositories;
using LmsBackend.Services.WaitListService.Dto;
using LmsBackend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsBackend.Services.WaitListService
{
    /// <summary>
    /// 
    /// </summary>
    public class WaitListAppService: AsyncCrudAppService<WaitList, WaitListDto,Guid>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public WaitListAppService(IRepository<WaitList, Guid> repository):base(repository) { 
        
        }
    }

}
