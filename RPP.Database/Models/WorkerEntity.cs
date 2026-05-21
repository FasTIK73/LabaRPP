using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RPP.Common.Enums;

namespace RPP.Database.Models;

[Table("Workers")]
public class WorkerEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [MaxLength(36)]
    public required string Id { get; set; }

    [Required]
    [MaxLength(200)]
    public required string FullName { get; set; }

    [Required]
    [MaxLength(20)]
    public required string PhoneNumber { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Email { get; set; }

    [Required]
    public WorkerPost Post { get; set; }

    [Required]
    public DateTime HireDate { get; set; }

    [Required]
    public DateTime BirthDate { get; set; }

    [Required]
    public double BaseRate { get; set; }

    public bool IsDeleted { get; set; } = false;

    // НОВОЕ ПОЛЕ ДЛЯ 5 ЛАБЫ
    public DateTime? DateOfDelete { get; set; }

    public ICollection<ReportEntity>? Reports { get; set; }
}