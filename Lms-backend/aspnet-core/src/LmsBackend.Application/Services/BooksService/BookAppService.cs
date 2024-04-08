using Abp.Application.Services;
using Abp.Domain.Repositories;
using LmsBackend.Services.BooksService.Dto;
using LmsBackend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp;
using Microsoft.EntityFrameworkCore;
using Abp.UI;

namespace LmsBackend.Services.BooksService
{
    /// <summary>
    /// 
    /// </summary>
    /*[AbpAuthorize]*/
    public class BookAppService : AsyncCrudAppService<Book, BookDto, Guid>
    {

      /// <summary>
      /// 
      /// </summary>
      /// <param name="repository"></param>
        public BookAppService(IRepository<Book, Guid> repository) : base(repository)
        {

        }
        // custom end points

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isbn"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<BookDto> GetBookByISBN(string isbn) {
            var book = await Repository.FirstOrDefaultAsync(b => b.ISBN == isbn);
            return ObjectMapper.Map<BookDto>(book);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="genre"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<BookDto>> GetBooksByGenre(string genre) {
            var books = await Repository.GetAllListAsync(b => b.Genre == genre);
            return ObjectMapper.Map<List<BookDto>>(books);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException"></exception>
        [HttpGet]
        public async Task<int> GetBookQuantity(Guid bookId)
        {
            // check if book exists in the database

            var bookRef = await Repository.FirstOrDefaultAsync(b => b.Id == bookId);
            if (bookRef == null)
            {
                throw new AbpException($" bookId is null");
            }

            // return the book quantity
            return bookRef.Quantity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="isIncrement"></param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException"></exception>
        [HttpPost]
        public async Task<int> AdjustBookQuantity(Guid bookId, bool isIncrement)
        {
            var bookRef = await Repository.FirstOrDefaultAsync(b => b.Id == bookId);
            if (bookRef == null)
            {
                throw new AbpException("BookId is null");
            }

            // increment book quantity if book exists

            if (isIncrement)
            {
                bookRef.Quantity++;
            }
            else
            {
                bookRef.Quantity--;
                bookRef.AccessFrequency++;

                if (bookRef.Quantity < 0)
                {
                    bookRef.Quantity = 0;
                }
            }

            // save changes
            await Repository.UpdateAsync(bookRef);

            // return the latest quantity

            return bookRef.Quantity;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="AbpException"></exception>
        [HttpGet]
        public async Task<BookDto> GetBookById(Guid bookId) {
            var book = await Repository.FirstOrDefaultAsync(b => b.Id == bookId);

            if (book == null) {
                throw new AbpException("No books available");
            }
            return ObjectMapper.Map<BookDto>(book);
        }

       /// <summary>
       /// 
       /// </summary>
       /// <returns></returns>
        [HttpGet]
        public async Task<BookDto> GetAllBooks() { 
            var books = await Repository.GetAllListAsync(); 
            return ObjectMapper.Map<BookDto>(books);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="author"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        [HttpGet]
        public async Task<List<BookDto>> SearchBooks(string title = "", string author = "") {

    
            var query =  Repository.GetAll();


            if (!string.IsNullOrWhiteSpace(title)) { 
                
                query =  query.Where(x => x.Title.Contains(title));
            }
            
            if (!string.IsNullOrWhiteSpace(author)) {
                query = query.Where(x => x.Author.Contains(author));
            }

            var books =  query.ToList();

           if(books.Count == 0) {
                throw new UserFriendlyException("No books found matching the search criteria.");
            }

            return ObjectMapper.Map<List<BookDto>>(books);

        }




        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<BookDto>> GetTrends() {

            var books = await Repository.GetAllListAsync();
            var sortedBooks = books.OrderByDescending(book => book.CreationTime).ToList();
            if (sortedBooks.Count > 0)
            {
                // Group books by genre
                var groupedByGenre = sortedBooks.GroupBy(book => book.Genre);

                // Initialize a list to store the selected books
                var selectedBooks = new List<Book>();

                // Select one book from each genre
                foreach (var group in groupedByGenre)
                {
                    var bookFromGenre = group.FirstOrDefault();
                    if (bookFromGenre != null)
                    {
                        selectedBooks.Add(bookFromGenre);
                    }
                }

                // If there are less than 10 genres, select additional recent books
                while (selectedBooks.Count < 10 && sortedBooks.Count > selectedBooks.Count)
                {
                    var additionalBook = sortedBooks[selectedBooks.Count];
                    selectedBooks.Add(additionalBook);
                }

                // Map selected books to BookDto and return
                return ObjectMapper.Map<List<BookDto>>(selectedBooks);
            }

            throw new AbpException("Error getting books");
        }
    }
}
