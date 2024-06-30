using Microsoft.EntityFrameworkCore;
using Notes.Data.Entities;

namespace Notes.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Note> Notes { get; set; }
    }
}
