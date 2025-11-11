using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bluesky.Models
{
    public enum TipoEvaluacion { Diagnostica = 1, Formativa = 2, Sumativa = 3 }

    [Table("evaluaciones")]
    public class Evaluacion
    {
        [Key] public int Id { get; set; }

        [Required] public int CursoId { get; set; }
        [ForeignKey(nameof(CursoId))] public virtual Curso Curso { get; set; }

        [Required, StringLength(150)]
        public string Titulo { get; set; }

        public TipoEvaluacion Tipo { get; set; } = TipoEvaluacion.Sumativa;

        // Configuración
        public int NumeroPreguntas { get; set; } = 15;
        public int TiempoMinutos { get; set; } = 30;
        public decimal PuntajeAprobacion { get; set; } = 60m; // 0..100

        // Versionado (para IA/autogeneradas)
        public int Version { get; set; } = 1;

        public bool Activa { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }
}
