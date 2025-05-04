namespace WebApp.ViewModels;

public class ProjectCardViewModel
{
    public string ProjectName { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string StatusName { get; set; } = null!;
    public string StatusSlug => StatusName.ToLower().Replace(" ", "-"); // används som CSS-klass
}

// help from chatgpt
