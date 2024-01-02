using System;
using System.Collections.Generic;

namespace DoAnWeb.Models;

public partial class CategoryProject
{
    public int CategoryPid { get; set; }

    public string? Name { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
