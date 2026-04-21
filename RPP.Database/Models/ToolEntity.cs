using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RPP.Database.Models;

[Table("Tools")]
public class ToolEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [MaxLength(36)]
    public required string Id { get; set; }

    [Required]
    [MaxLength(100)]
    public required string ToolName { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public bool IsAvailable { get; set; } = true;

    // Историчность типа 3
    [MaxLength(100)]
    public string? PreviousToolName { get; set; }

    // Navigation properties
    public ICollection<ReportEntity>? Reports { get; set; }
}