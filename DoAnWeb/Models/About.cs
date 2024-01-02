using System;
using System.Collections.Generic;

namespace DoAnWeb.Models;

public partial class About
{
    public int AboutId { get; set; }

    public string? Title { get; set; }

    public string? Image { get; set; }

    public bool IsActive { get; set; }

    public string? AboutUs { get; set; }

    public string? OurStory { get; set; }
}
