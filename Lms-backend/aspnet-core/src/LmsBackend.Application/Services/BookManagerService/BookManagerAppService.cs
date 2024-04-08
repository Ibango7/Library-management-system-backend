using Abp.Application.Services;
using Abp.Domain.Repositories;
using LmsBackend.Services.BookManagerService.Dto;
using LmsBackend.CustomServices.Books;
using LmsBackend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Abp.Domain.Entities;
using System.Net;
using LmsBackend.Authorization.Users;
using LmsBackend.Services.BooksService;
using Abp;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;


namespace LmsBackend.Services.BookManagerService
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    [AbpAuthorize]
    public class BookManagerAppService: AsyncCrudAppService<BookManagement, BookManagementDto, Guid>
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly BookAppService _bookAppService;
      /// <summary>
      /// 
      /// </summary>
      /// <param name="repository"></param>
      /// <param name="userRepository"></param>
      /// <param name="bookAppService"></param>
        public BookManagerAppService(IRepository<BookManagement,Guid> repository, IRepository<User, long> userRepository, BookAppService bookAppService) : base(repository) {
            _bookAppService = bookAppService;
            _userRepository = userRepository;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<bool> isRented(Guid bookId, long userId) {

            // Check if user has already rented the same book
            var existingBooking = await Repository.FirstOrDefaultAsync(b => b.BookId == bookId && b.UserId == userId && b.Returned == false);
            if (existingBooking == null)
            {
                return false;

            }

            if (existingBooking != null)
            {
                return true;
            }

            return false;
        }
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>

        [HttpPost]
        public async Task<BookManagementDto> RentBook(Guid bookId, long userId) {
            // check if the book quantity is greater than 0
            var bookQuantity =  await _bookAppService.GetBookQuantity(bookId);
              
            if(bookQuantity <= 0)
            {
                throw new AbpException($"Book is not available for rent. Quantity: {bookQuantity}");

            }


            // check if user has already booked the same book

            var existingBooking = await Repository.FirstOrDefaultAsync(b => b.BookId == bookId && b.UserId == userId);
 
            if(existingBooking != null)
            {
                throw new AbpException($"User {userId} This book has already been booked");

            }

            var newBookManagement = new BookManagement
            {
                UserId = userId,
                BookId = bookId,
                DateBorrowed = DateTime.Now,
                Returned = false,
                Overdue = false,    

            };

            // decrement quantity of books available
            await _bookAppService.AdjustBookQuantity(bookId, isIncrement:false);

            // Add book management to the database
            await Repository.InsertAsync(newBookManagement);
          
            return ObjectMapper.Map<BookManagementDto>(newBookManagement);

        }

       
        [HttpGet]
        public async Task<List<BookManagementDto>> GetRentedBooksByUser(long userId)
        {

            var rentedBooks =  Repository.GetAllIncluding( x=>x.Book).Where(x => x.UserId==userId);

            return ObjectMapper.Map<List<BookManagementDto>>(rentedBooks);

            

        }

    }
}
