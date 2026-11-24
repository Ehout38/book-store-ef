using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<BookImage> BookImages => Set<BookImage>();
}
