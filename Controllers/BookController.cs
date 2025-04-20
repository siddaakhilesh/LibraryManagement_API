using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Models;
using LibraryAPI.Data;
using System;
using LibraryAPI.Services;
using System.Threading.Tasks;

namespace LibraryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<BooksController> _logger;
    private readonly IDbLogger _dbLogger;

    public BooksController(AppDbContext context, ILogger<BooksController> logger, IDbLogger dbLogger)
    {
        _context = context;
        _logger = logger;
        _dbLogger = dbLogger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
    {
        _logger.LogInformation("Getting all books");
        await _dbLogger.LogAsync("Information", "Fetching all books from databse");
        return Ok(_context.Books.ToList());
    }

    [HttpPost]
    public async Task<ActionResult<Book>> AddBook(Book book)
    {
        _context.Books.Add(book);
        _context.SaveChanges();
        _logger.LogInformation("Book added: {Title}", book.Title);
        await _dbLogger.LogAsync("Informatio", $"Book added: {book.Title}");
        return CreatedAtAction(nameof(GetBooks), new { id = book.Id }, book);
    }
    [HttpGet("{id}")]
    public ActionResult<Book> GetBookById(int id)
    {
        var book = _context.Books.Find(id);

        if (book == null)
        {
            _logger.LogWarning("Book with ID {Id} not found", id);
            return NotFound();
        }

        _logger.LogInformation("Fetched book with ID {Id}", id);
        return Ok(book);
    }

    [HttpGet("title/{title}")]
    public ActionResult<Book> GetBookByTitle(string title)
    {
        var book = _context.Books
            .FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

        if (book == null)
        {
            _logger.LogWarning("Book with title '{Title}' not found", title);
            return NotFound();
        }

        _logger.LogInformation("Fetched book with title '{Title}'", title);
        return Ok(book);
    }

}
