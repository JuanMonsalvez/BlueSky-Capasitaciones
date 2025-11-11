using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bluesky.Models
{
    [Table("usuario_cursos")]
    public class UsuarioCurso
    {
        [Key] public int Id { get; set; }

        [Required] public int UsuarioId { get; set; }
        [ForeignKey(nameof(UsuarioId))] public virtual Usuario Usuario { get; set; }

        [Required] public int CursoId { get; set; }
        [ForeignKey(nameof(CursoId))] public virtual Curso Curso { get; set; }

        public DateTime FechaInscripcion { get; set; } = DateTime.UtcNow;

        // 0..100
        public decimal Progreso { get; set; } = 0m;

        public bool Completado { get; set; } = false;
        public DateTime? FechaComplecion { get; set; }
    }
}
