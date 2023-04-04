namespace PerformanceManagementSystem.Data.Views.MenuItems;

public class UserMenuItemResponseDto
{
    public string AspPage { get; set; } = null!;
    public string AspArea { get; set; } = null!;
    public string Class { get; set; } = null!;
    public string Title { get; set; } = null!;
    public bool AriaCurrent { get; set; }
    public int DisplayOrder { get; set; }
}