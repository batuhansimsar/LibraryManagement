using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using LibraryManagement.Web.Controllers;
using LibraryManagement.Web.Data;
using LibraryManagement.Web.Models;

namespace LibraryManagement.Tests
{
    public class BooksControllerTests
    {
        private LibraryContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new LibraryContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfBooks()
        {
            // Arrange
            var context = GetDatabaseContext();
            context.Books.Add(new Book { Title = "Test Book 1", Author = "Author 1" });
            context.Books.Add(new Book { Title = "Test Book 2", Author = "Author 2" });
            await context.SaveChangesAsync();

            var controller = new BooksController(context);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Book>>(viewResult.ViewData.Model);
            Assert.Equal(2, ((List<Book>)model).Count);
        }

        [Fact]
        public async Task Create_ReturnsRedirectAndAddsBook_WhenModelStateIsValid()
        {
            // Arrange
            var context = GetDatabaseContext();
            var controller = new BooksController(context);
            var newBook = new Book { Title = "New Book", Author = "New Author", ISBN = "123456789", PublishedDate = DateTime.Now };

            // Act
            var result = await controller.Create(newBook);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal(1, await context.Books.CountAsync());
        }

        [Fact]
        public async Task Create_ReturnsView_WhenModelStateIsInvalid()
        {
            // Arrange
            var context = GetDatabaseContext();
            var controller = new BooksController(context);
            controller.ModelState.AddModelError("Title", "Required");
            var newBook = new Book { Author = "New Author" }; // Missing Title

            // Act
            var result = await controller.Create(newBook);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(newBook, viewResult.Model);
            Assert.Equal(0, await context.Books.CountAsync());
        }
    }
}
