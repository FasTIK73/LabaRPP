using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RPP.Common.Enums;

namespace RPP.Database.Models;

[Table("Reports")]
public class ReportEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [MaxLength(36)]
    public required string Id { get; set; }

    [Required]
    [MaxLength(36)]
    public required string HomeId { get; set; }

    [Required]
    [MaxLength(36)]
    public required string WorkTypeId { get; set; }

    [Required]
    [MaxLength(36)]
    public required string WorkerId { get; set; }

    [Required]
    [MaxLength(36)]
    public required string ToolId { get; set; }

    [Required]
    public DateTime WorkDate { get; set; }

    [Required]
    public double WorkVolume { get; set; }

    [Required]
    public ReportStatus Status { get; set; }

    [Required]
    public double TotalCost { get; set; }

    [ForeignKey(nameof(HomeId))]
    public HomeEntity? Home { get; set; }

    [ForeignKey(nameof(WorkTypeId))]
    public WorkTypeEntity? WorkType { get; set; }

    [ForeignKey(nameof(WorkerId))]
    public WorkerEntity? Worker { get; set; }

    [ForeignKey(nameof(ToolId))]
    public ToolEntity? Tool { get; set; }
}