using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bluesky.Models
{
    [Table("intentos_evaluacion")]
    public class IntentoEvaluacion
    {
        [Key] public int Id { get; set; }

        [Required] public int UsuarioId { get; set; }
        [ForeignKey(nameof(UsuarioId))] public virtual Usuario Usuario { get; set; }

        [Required] public int EvaluacionId { get; set; }
        [ForeignKey(nameof(EvaluacionId))] public virtual Evaluacion Evaluacion { get; set; }

        public DateTime FechaInicio { get; set; } = DateTime.UtcNow;
        public DateTime? FechaFin { get; set; }

        public decimal PuntajeObtenido { get; set; } = 0m; // 0..100
        public bool Aprobado { get; set; } = false;

        public string DetalleJson { get; set; } // opcional: snapshot IA / tracking
    }
}
