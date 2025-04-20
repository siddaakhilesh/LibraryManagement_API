using Microsoft.EntityFrameworkCore;
using LibraryAPI.Models;
using System.Collections.Generic;

namespace LibraryAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<LogEntry> Logs => Set<LogEntry>();

}
