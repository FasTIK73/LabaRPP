using System.ComponentModel.DataAnnotations;

namespace RPP.WebApi.Models.BindingModels;

public class PostBindingModel
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "Название должности обязательно")]
    public string? PostName { get; set; }

    [Required(ErrorMessage = "Тип должности обязателен")]
    public int PostType { get; set; }

    [Required(ErrorMessage = "Конфигурация обязательна")]
    public string? ConfigurationJson { get; set; }
}