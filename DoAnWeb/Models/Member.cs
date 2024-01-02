using System;
using System.Collections.Generic;

namespace DoAnWeb.Models;

public partial class Member
{
    public int MemberId { get; set; }

    public string? Name { get; set; }

    public string? WorkPosition { get; set; }

    public string? Detail { get; set; }

    public bool IsActive { get; set; }

    public string? Link { get; set; }

    public string? Image { get; set; }

    public string? Message { get; set; }
}
