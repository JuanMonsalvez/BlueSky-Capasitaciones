using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bluesky.Models
{
    public enum NivelCurso { Basico = 1, Intermedio = 2, Avanzado = 3 }

    [Table("cursos")]
    public class Curso
    {
        [Key] public int Id { get; set; }

        [Required, StringLength(150)]
        public string Titulo { get; set; }

        [StringLength(2000)]
        public string Descripcion { get; set; }

        public int? AreaId { get; set; }
        [ForeignKey(nameof(AreaId))] public virtual Area Area { get; set; }

        public int? InstructorId { get; set; }
        [ForeignKey(nameof(InstructorId))] public virtual Usuario Instructor { get; set; }

        public int DuracionHoras { get; set; } = 0;
        public NivelCurso Nivel { get; set; } = NivelCurso.Basico;

        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaActualizacion { get; set; }
    }
}
