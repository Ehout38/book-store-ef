using System.ComponentModel.DataAnnotations;

namespace BookStore.Models;

public class Comment
{
    public int Id { get; set; }

    [Required, StringLength(60)]
    public string AuthorName { get; set; } = string.Empty;

    [Required, StringLength(1000)]
    public string Text { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int BookId { get; set; }
    public Book? Book { get; set; }
}
