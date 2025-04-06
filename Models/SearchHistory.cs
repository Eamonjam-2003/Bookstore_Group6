using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("SearchHistories")]
public class SearchHistory
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Search query is required.")]
    [StringLength(255, ErrorMessage = "Search query cannot exceed 255 characters.")]
    public string Query { get; set; }

    [Required]
    public DateTime SearchDate { get; set; }
}
