using BookStore.Data;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers;

public class BooksController(AppDbContext context, IWebHostEnvironment environment) : Controller
{
    public async Task<IActionResult> Index()
    {
        var books = await context.Books.Include(x => x.Images).ToListAsync();
        return View(books);
    }

    public async Task<IActionResult> Details(int id)
    {
        var book = await context.Books
            .Include(x => x.Images)
            .Include(x => x.Comments.OrderByDescending(c => c.CreatedAt))
            .FirstOrDefaultAsync(x => x.Id == id);

        if (book is null)
        {
            return NotFound();
        }

        return View(new BookDetailsViewModel { Book = book });
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Book book, List<IFormFile> images)
    {
        if (!ModelState.IsValid)
        {
            return View(book);
        }

        var folder = Path.Combine(environment.WebRootPath, "images", "books");
        Directory.CreateDirectory(folder);

        foreach (var image in images.Where(x => x.Length > 0 && x.ContentType.StartsWith("image/")))
        {
            var extension = Path.GetExtension(image.FileName);
            var fileName = $"{Guid.NewGuid():N}{extension}";
            var fullPath = Path.Combine(folder, fileName);

            await using var stream = System.IO.File.Create(fullPath);
            await image.CopyToAsync(stream);
            book.Images.Add(new BookImage { FilePath = $"/images/books/{fileName}" });
        }

        context.Books.Add(book);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id = book.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddComment(int bookId, Comment comment)
    {
        if (!await context.Books.AnyAsync(x => x.Id == bookId))
        {
            return NotFound();
        }

        ModelState.Remove(nameof(Comment.Book));
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Enter your name and comment.";
            return RedirectToAction(nameof(Details), new { id = bookId });
        }

        comment.BookId = bookId;
        comment.CreatedAt = DateTime.UtcNow;
        context.Comments.Add(comment);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = bookId });
    }
}
