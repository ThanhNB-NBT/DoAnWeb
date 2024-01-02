using System;
using System.Collections.Generic;

namespace DoAnWeb.Models;

public partial class CategoryBlog
{
    public int CategoryId { get; set; }

    public string? Name { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();
}
