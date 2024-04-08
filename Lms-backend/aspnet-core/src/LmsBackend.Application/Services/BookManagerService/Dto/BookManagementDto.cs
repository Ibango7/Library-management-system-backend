using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using LmsBackend.Authorization.Users;
using LmsBackend.Entities;
using LmsBackend.Services.BooksService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LmsBackend.Services.BookManagerService.Dto 
{
    [AutoMap(typeof(BookManagement))]
    public class BookManagementDto: EntityDto<Guid>
    {
        public virtual bool Returned { get; set; }
        public virtual bool Overdue { get; set; }
        public virtual DateTime DateBorrowed { get; set; }


        public virtual BookDto Book { get; set; }

        public virtual long UserId { get; set; }
        public virtual User User { get; set; }

    }
}
