using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSyncWorkerService
{
    public class BookDetails
    {

        public BookDetails() { }
        public BookDetails(string title, string author, string description, string ISBN, string imageUrl, int shelfNumber) {
            Title = title;
            this.Author = author;
            Description = description;
            this.ISBN = ISBN;
            ImageUrl = imageUrl;
            this.Quantity = 50; // Default quantity
            this.Shelf = shelfNumber;
            this.AccessFrequency = 0; // Default quantity
        }

        public override string ToString()
        {
            return $"Title: {Title}\nGenre: {Genre}\nAuthors: {Author}\nDescription: {Description}\nISBN: {Id}\nThumbnailUrl: {ImageUrl}";
        }

        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Description { get; set; }
        public string Content { get; set; }
        public Guid Id { get; set; }
        public string ISBN { get; set; }
        public string Genre { get; set; }
        public string? ImageUrl { get; set; }
        public int Quantity {  get; set; }
        public int Shelf { get; set; }
        public int AccessFrequency {  get; set; }
    }
}
