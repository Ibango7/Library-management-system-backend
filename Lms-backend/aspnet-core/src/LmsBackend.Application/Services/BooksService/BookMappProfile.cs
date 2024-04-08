using AutoMapper;
using LmsBackend.Entities;
using LmsBackend.Services.BookManagerService.Dto;
using LmsBackend.Services.BooksService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsBackend.Services.BooksService
{
    public class BookMappProfile:Profile
    {
        public BookMappProfile()
        {
            CreateMap<BookDto, Book>();
             

            CreateMap<Book, BookDto>();

           /* CreateMap<BookManagement, BookManagementDto>()
                .ForMember(x=> x.Book)*/
        }
    }
}
