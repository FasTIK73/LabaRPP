using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RPP.Common.Infrastructure.PostConfigurations;

namespace RPP.Database.Models;

[Table("Posts")]
public class PostEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public string PostId { get; set; }

    [Required]
    public string PostName { get; set; }

    [Required]
    public int PostType { get; set; }

    [Required]
    [Column(TypeName = "jsonb")]
    public PostConfiguration Configuration { get; set; }

    public bool IsActual { get; set; }

    public DateTime ChangeDate { get; set; }
}