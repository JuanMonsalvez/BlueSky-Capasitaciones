using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bluesky.Models
{
    public enum TipoNotificacion { Sistema = 1, Curso = 2, Evaluacion = 3 }

    [Table("notificaciones")]
    public class Notificacion
    {
        [Key] public int Id { get; set; }

        [Required] public int UsuarioId { get; set; }
        [ForeignKey(nameof(UsuarioId))] public virtual Usuario Usuario { get; set; }

        public TipoNotificacion Tipo { get; set; } = TipoNotificacion.Sistema;

        [Required, StringLength(150)]
        public string Titulo { get; set; }

        [StringLength(1000)]
        public string Mensaje { get; set; }

        public bool Leida { get; set; } = false;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }
}
