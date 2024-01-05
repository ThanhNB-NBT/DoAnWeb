using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnWeb.Models;

public partial class Menu
{
    public int MenuId { get; set; }

    public string? MenuName { get; set; }

    public bool IsActive { get; set; }

    public string? ControllerName { get; set; }

    public string? ActionName { get; set; }

    public int? Levels { get; set; }

    public int? ParentId { get; set; }

    public string? Link { get; set; }

    public int? MenuOrder { get; set; }

    public int? Position { get; set; }
    [NotMapped] // Đánh dấu thuộc tính này không phải là một cột trong cơ sở dữ liệu
    public string? ParentName { get; set; } = string.Empty;
}
