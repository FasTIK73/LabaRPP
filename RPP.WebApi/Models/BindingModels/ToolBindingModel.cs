using System.ComponentModel.DataAnnotations;

namespace RPP.WebApi.Models.BindingModels;

/// <summary>
/// Модель для создания/обновления инструмента
/// </summary>
public class ToolBindingModel
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "Название инструмента обязательно")]
    [MinLength(2, ErrorMessage = "Название должно содержать минимум 2 символа")]
    [MaxLength(100, ErrorMessage = "Название не должно превышать 100 символов")]
    public string? ToolName { get; set; }

    [MaxLength(500, ErrorMessage = "Описание не должно превышать 500 символов")]
    public string? Description { get; set; }

    public bool IsAvailable { get; set; } = true;
}