namespace RPP.WebApi.Models.ViewModels;

public class PostViewModel
{
    public string Id { get; set; } = string.Empty;
    public string PostName { get; set; } = string.Empty;
    public int PostType { get; set; }
    public string Configuration { get; set; } = string.Empty;
}