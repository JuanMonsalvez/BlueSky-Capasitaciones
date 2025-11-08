using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bluesky.Models
{
    [Table("usuarios")]
    public class Usuario
    {
        [Key] public int Id { get; set; }

        [Required, StringLength(150)]
        public string NombreCompleto { get; set; }

        [Required, StringLength(150)]
        public string Correo { get; set; }

        [Required, StringLength(256)]
        public string PasswordHash { get; set; }

        public int? RolId { get; set; }
        [ForeignKey("RolId")] public virtual Rol Rol { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
        public bool Activo { get; set; } = true;
    }
}
