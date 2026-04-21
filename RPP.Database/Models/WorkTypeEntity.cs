using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RPP.Common.Enums;

namespace RPP.Database.Models;

[Table("WorkTypes")]
public class WorkTypeEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [MaxLength(36)]
    public required string Id { get; set; }

    [Required]
    [MaxLength(200)]
    public required string WorkName { get; set; }

    [Required]
    public MeasurementUnit Unit { get; set; }

    [Required]
    public double PricePerUnit { get; set; }

    public double? PreviousPrice { get; set; }

    public DateTime? PriceChangeDate { get; set; }

    public ICollection<ReportEntity>? Reports { get; set; }
}