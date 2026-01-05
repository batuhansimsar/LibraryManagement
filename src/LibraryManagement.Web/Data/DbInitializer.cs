using LibraryManagement.Web.Models;
using System;
using System.Linq;

namespace LibraryManagement.Web.Data
{
    public static class DbInitializer
    {
        public static void Initialize(LibraryContext context)
        {
            // Look for any books.
            if (context.Books.Any())
            {
                return;   // DB has been seeded
            }

            var books = new Book[]
            {
                new Book { Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", ISBN = "9780743273565", PublishedDate = new DateTime(1925, 4, 10) },
                new Book { Title = "1984", Author = "George Orwell", ISBN = "9780451524935", PublishedDate = new DateTime(1949, 6, 8) },
                new Book { Title = "The Catcher in the Rye", Author = "J.D. Salinger", ISBN = "9780316769488", PublishedDate = new DateTime(1951, 7, 16) },
                new Book { Title = "To Kill a Mockingbird", Author = "Harper Lee", ISBN = "9780061120084", PublishedDate = new DateTime(1960, 7, 11) },
                new Book { Title = "Pride and Prejudice", Author = "Jane Austen", ISBN = "9781503290563", PublishedDate = new DateTime(1813, 1, 28) }
            };

            context.Books.AddRange(books);
            context.SaveChanges();
        }
    }
}
