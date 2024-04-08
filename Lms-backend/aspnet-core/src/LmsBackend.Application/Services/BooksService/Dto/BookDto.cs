using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LmsBackend.Entities;


namespace LmsBackend.Services.BooksService.Dto 
{
    [AutoMap(typeof(Book))]
    public class BookDto : EntityDto<Guid>
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string ImageUrl { get; set; }
        public int AccessFrequency { get; set; }
        public int Quantity { get; set; }
        public int Shelf { get; set; }

    }
}
