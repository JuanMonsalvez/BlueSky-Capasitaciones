using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bluesky.Models
{
    [Table("areas")]
    public class Area
    {
        [Key] public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; }
    }
}
