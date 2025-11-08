using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bluesky.Models
{
    [Table("roles")]
    public class Rol
    {
        [Key] public int Id { get; set; }

        [Required, StringLength(50)]
        public string Nombre { get; set; } // "Admin", "Usuario"
    }
}
