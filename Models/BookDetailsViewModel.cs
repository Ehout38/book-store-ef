namespace BookStore.Models;

public class BookDetailsViewModel
{
    public Book Book { get; set; } = null!;
    public Comment Comment { get; set; } = new();
}
