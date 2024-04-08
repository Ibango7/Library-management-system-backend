using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsBackend.Services.BookManagerService.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class RentedBookDto: EntityDto<Guid>
    {

        public long UserId { get; set; }
        public Guid BookId { get; set; }
        public DateTime DateBorrowed { get; set; }
        public bool Returned { get; set; }
        public bool Overdue { get; set; }
        // Additional properties for book information
        public string BookTitle { get; set; }
        public string Author { get; set; }
    }
}
