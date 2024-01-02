using System;
using System.Collections.Generic;

namespace DoAnWeb.Models;

public partial class AdminMenu
{
    public long MenuAdminId { get; set; }

    public string? ItemName { get; set; }

    public int? ItemLevels { get; set; }

    public int? ParentLevels { get; set; }

    public int? ItemOrder { get; set; }

    public bool IsActive { get; set; }

    public string? AreaName { get; set; }

    public string? ControllerName { get; set; }

    public string? ActionName { get; set; }

    public string? Icon { get; set; }
}
