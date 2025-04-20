using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Data;
using LibraryAPI.Models;

namespace LibraryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LogsController : ControllerBase
{
    private readonly AppDbContext _context;

    public LogsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<LogEntry>> GetLogs()
    {
        return Ok(_context.Logs.OrderByDescending(l => l.Timestamp).ToList());
    }
}
