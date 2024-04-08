using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using LmsBackend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsBackend.Services.LibraryEventService.Dto
{
    [AutoMap(typeof(LibraryEvent))]
    public class LibraryEventDto:EntityDto<Guid>
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual string ImageUrl { get; set; }

    }
}
