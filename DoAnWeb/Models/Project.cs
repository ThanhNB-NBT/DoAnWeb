using System;
using System.Collections.Generic;

namespace DoAnWeb.Models;

public partial class Project
{
    public int ProjectId { get; set; }

    public string? ProjectName { get; set; }

    public bool IsActive { get; set; }

    public string? Image { get; set; }

    public int? CategoryPid { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? Title { get; set; }

    public string? Detail { get; set; }

    public string? Client { get; set; }

    public string? Link { get; set; }

    public virtual CategoryProject? CategoryP { get; set; }
}
