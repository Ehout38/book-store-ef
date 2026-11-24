using BookStore.Models;

namespace BookStore.Data;

public static class SeedData
{
    public static void Initialize(AppDbContext context)
    {
        if (context.Books.Any())
        {
            return;
        }

        context.Books.AddRange(
            new Book { Title = "The Hobbit", Author = "J. R. R. Tolkien", Genre = "Fantasy", Price = 420 },
            new Book { Title = "1984", Author = "George Orwell", Genre = "Dystopia", Price = 310 },
            new Book { Title = "Clean Code", Author = "Robert C. Martin", Genre = "Programming", Price = 850 });

        context.SaveChanges();
    }
}
