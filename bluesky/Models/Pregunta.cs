using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bluesky.Models
{
    public enum DificultadPregunta { Facil = 1, Media = 2, Dificil = 3 }

    [Table("preguntas")]
    public class Pregunta
    {
        [Key] public int Id { get; set; }

        [Required] public int EvaluacionId { get; set; }
        [ForeignKey(nameof(EvaluacionId))] public virtual Evaluacion Evaluacion { get; set; }

        [Required, StringLength(1000)]
        public string Enunciado { get; set; }

        [StringLength(200)]
        public string Categoria { get; set; } // tema/competencia

        public DificultadPregunta Dificultad { get; set; } = DificultadPregunta.Media;

        // Soporta single/multi response si más adelante agregas una tabla de mapeo
        public bool MultipleRespuesta { get; set; } = false;

        public int Orden { get; set; } = 1;
        public bool Activa { get; set; } = true;
    }
}
