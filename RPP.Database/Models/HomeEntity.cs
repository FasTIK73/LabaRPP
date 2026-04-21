using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RPP.Common.Enums;

namespace RPP.Database.Models;

[Table("Homes")]
public class HomeEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [MaxLength(36)]
    public required string Id { get; set; }

    [Required]
    [MaxLength(36)]
    public required string ClientId { get; set; }

    [Required]
    [MaxLength(500)]
    public required string Address { get; set; }

    [Required]
    public double Area { get; set; }

    [Required]
    public HomeType Type { get; set; }

    [Required]
    public HomeStatus Status { get; set; }

    [ForeignKey(nameof(ClientId))]
    public ClientEntity? Client { get; set; }

    public ICollection<ReportEntity>? Reports { get; set; }
}