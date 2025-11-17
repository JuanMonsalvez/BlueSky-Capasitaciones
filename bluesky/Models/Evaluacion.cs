using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bluesky.Models
{
    public enum TipoEvaluacion
    {
        Diagnostica = 1,
        Formativa = 2,
        Sumativa = 3
    }

    // 🔐 Política de intentos
    public enum PoliticaIntentosEvaluacion
    {
        MaximoPorDia = 1,   // 3 intentos por ventana (por defecto)
        Ilimitado = 2,    // sin límite
        Bloqueado = 3     // nadie puede rendir
    }

    [Table("evaluaciones")]
    public class Evaluacion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CursoId { get; set; }

        [ForeignKey(nameof(CursoId))]
        public virtual Curso Curso { get; set; }

        [Required, StringLength(150)]
        public string Titulo { get; set; }

        public TipoEvaluacion Tipo { get; set; } = TipoEvaluacion.Sumativa;

        // Configuración de la evaluación
        public int NumeroPreguntas { get; set; } = 15;
        public int TiempoMinutos { get; set; } = 30;
        public decimal PuntajeAprobacion { get; set; } = 60m; // 0..100

        // Versionado (IA / cambios futuros)
        public int Version { get; set; } = 1;

        public bool Activa { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // 🔐 NUEVO: política de intentos
        public PoliticaIntentosEvaluacion PoliticaIntentos { get; set; } =
            PoliticaIntentosEvaluacion.MaximoPorDia;

        /// <summary>
        /// Máximo de intentos dentro de la ventana (por defecto 3).
        /// Aplica cuando PoliticaIntentos == MaximoPorDia.
        /// </summary>
        public int MaxIntentosPorDia { get; set; } = 3;

        /// <summary>
        /// Ventana de cooldown en horas (por defecto 24h).
        /// </summary>
        public int CooldownHoras { get; set; } = 24;
    }
}
