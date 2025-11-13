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

    // Nueva política de intentos por evaluación
    public enum PoliticaIntentosEvaluacion
    {
        Ilimitado = 0,          // Sin límite
        MaximoPorDia = 1,       // Máx. N intentos por día (MaxIntentosPorDia)
        IlimitadoConCooldown = 2, // Intentos ilimitados pero con cooldown (CooldownHoras)
        Bloqueado = 3           // No permite más intentos
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

        // Configuración básica
        public int NumeroPreguntas { get; set; } = 15;
        public int TiempoMinutos { get; set; } = 30;
        public decimal PuntajeAprobacion { get; set; } = 60m; // 0..100

        // ------------------------------
        //  NUEVA CONFIGURACIÓN DE INTENTOS
        // ------------------------------
        public PoliticaIntentosEvaluacion PoliticaIntentos { get; set; } =
            PoliticaIntentosEvaluacion.Ilimitado;
        public int MaxIntentosPorDia { get; set; } = 3;

        public int CooldownHoras { get; set; } = 24;

        // Versionado (para IA/autogeneradas)
        public int Version { get; set; } = 1;

        public bool Activa { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }
}
