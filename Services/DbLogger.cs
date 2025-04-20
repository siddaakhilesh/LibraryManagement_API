using LibraryAPI.Data;
using LibraryAPI.Models;

namespace LibraryAPI.Services;

public interface IDbLogger
{
    Task LogAsync(string level, string message, string? exception = null);
}

public class DbLogger : IDbLogger
{
    private readonly AppDbContext _context;

    public DbLogger(AppDbContext context)
    {
        _context = context;
    }

    public async Task LogAsync(string level, string message, string? exception = null)
    {
        var log = new LogEntry
        {
            Timestamp = DateTime.UtcNow,
            Level = level,
            Message = message,
            Exception = exception
        };

        _context.Logs.Add(log);
        await _context.SaveChangesAsync();
    }
}
