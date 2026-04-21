using System.ComponentModel.DataAnnotations;
using RPP.Common.Enums;

namespace RPP.WebApi.Models.BindingModels;

/// <summary>
/// Модель для создания отчета о работе
/// </summary>
public class ReportBindingModel
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "ID помещения обязателен")]
    public string? HomeId { get; set; }

    [Required(ErrorMessage = "ID типа работы обязателен")]
    public string? WorkTypeId { get; set; }

    [Required(ErrorMessage = "ID работника обязателен")]
    public string? WorkerId { get; set; }

    [Required(ErrorMessage = "ID инструмента обязателен")]
    public string? ToolId { get; set; }

    [Required(ErrorMessage = "Дата работы обязательна")]
    public DateTime WorkDate { get; set; }

    [Required(ErrorMessage = "Объем работ обязателен")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Объем должен быть больше 0")]
    public double WorkVolume { get; set; }

    [Required(ErrorMessage = "Статус обязателен")]
    public ReportStatus Status { get; set; }
}