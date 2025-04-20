using Xunit;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LibraryAPI.Controllers;
using LibraryAPI.Models;
using LibraryAPI.Data;
using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Services;

namespace LibraryAPI.Tests;

public class BooksControllerTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Unique for each test
            .Options;
        return new AppDbContext(options);
    }

    private IDbLogger GetDbLogger(AppDbContext context)
    {
        return new DbLogger(context);
    }

    [Fact]
    public async Task GetBooks_ReturnsOkResultAndLogs()
    {
        // Arrange
        var context = GetDbContext();
        context.Books.Add(new Book { Title = "Test Book", Author = "Author", Year = 2023 });
        context.SaveChanges();

        var logger = new LoggerFactory().CreateLogger<BooksController>();
        var dbLogger = GetDbLogger(context);
        var controller = new BooksController(context, logger, dbLogger);

        // Act
        var result = await controller.GetBooks();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Single((List<Book>)okResult.Value);

        var log = context.Logs.FirstOrDefault();
        Assert.NotNull(log);
        Assert.Contains("Fetching all books", log.Message);
    }

    [Fact]
    public async Task AddBook_ReturnsCreatedAndLogs()
    {
        // Arrange
        var context = GetDbContext();
        var logger = new LoggerFactory().CreateLogger<BooksController>();
        var dbLogger = GetDbLogger(context);
        var controller = new BooksController(context, logger, dbLogger);

        var book = new Book { Title = "New Book", Author = "Tester", Year = 2024 };

        // Act
        var result = await controller.AddBook(book);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal("New Book", ((Book)createdResult.Value!).Title);

        var log = context.Logs.FirstOrDefault();
        Assert.NotNull(log);
        Assert.Contains("Book added", log.Message);
    }
}
