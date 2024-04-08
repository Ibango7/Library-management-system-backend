using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using LmsBackend.Authorization.Users;
using LmsBackend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsBackend.Services.WaitListService.Dto
{
    [AutoMap(typeof(WaitList))]
    public class WaitListDto:EntityDto<Guid>
    {
        // Foreign keys
        public virtual long UserId { get; set; }
        public virtual User User { get; set; }

        public virtual Guid BookId { get; set; }
        public virtual Book Book { get; set; }
    }
}
