using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bluesky.Models
{
    // Alineado con el grid: 0=Básico, 1=Intermedio, 2=Avanzado
    public enum NivelCurso { Basico = 0, Intermedio = 1, Avanzado = 2 }

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

        // ✅ NUEVO: se guarda la RUTA relativa de la imagen (p. ej. ~/Uploads/cursos/12/portada.jpg)
        [StringLength(300)]
        public string PortadaUrl { get; set; }

        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaActualizacion { get; set; }
    }
}
