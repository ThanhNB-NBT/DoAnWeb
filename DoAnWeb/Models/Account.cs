using System;
using System.Collections.Generic;

namespace DoAnWeb.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? FullName { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public int? RoleId { get; set; }

    public bool IsActive { get; set; }
    public string? Image { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual Role? Role { get; set; }
}
