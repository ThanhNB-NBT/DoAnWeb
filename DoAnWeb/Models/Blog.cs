using System;
using System.Collections.Generic;

namespace DoAnWeb.Models;

public partial class Blog
{
    public int BlogId { get; set; }

    public string? Title { get; set; }

    public string? Alias { get; set; }

    public string? Description { get; set; }

    public string? Detail { get; set; }

    public string? Image { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public string? Tags { get; set; }

    public int? AccountId { get; set; }

    public bool IsActive { get; set; }

    public int? CategoryId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<BlogComment> BlogComments { get; set; } = new List<BlogComment>();

    public virtual CategoryBlog? Category { get; set; }
}
