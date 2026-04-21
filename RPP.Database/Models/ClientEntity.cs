using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RPP.Common.Enums;  // ИЗМЕНЕНО

namespace RPP.Database.Models;

public class ClientEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [MaxLength(36)]
    public required string Id { get; set; }

    [Required]
    [MaxLength(200)]
    public required string Name { get; set; }

    [Required]
    [MaxLength(500)]
    public required string Address { get; set; }

    [Required]
    [MaxLength(20)]
    public required string PhoneNumber { get; set; }

    [Required]
    public DateTime RegistrationDate { get; set; }

    // Navigation properties
    public ICollection<HomeEntity>? Homes { get; set; }
}