using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bluesky.Models
{
    [Table("intentos_respuesta")]
    public class IntentoRespuesta
    {
        [Key] public int Id { get; set; }

        [Required] public int IntentoEvaluacionId { get; set; }
        [ForeignKey(nameof(IntentoEvaluacionId))] public virtual IntentoEvaluacion Intento { get; set; }

        [Required] public int PreguntaId { get; set; }
        [ForeignKey(nameof(PreguntaId))] public virtual Pregunta Pregunta { get; set; }

        public int? AlternativaId { get; set; }
        [ForeignKey(nameof(AlternativaId))] public virtual Alternativa Alternativa { get; set; }

        public bool EsCorrecta { get; set; } = false;
        public decimal Puntaje { get; set; } = 0m; // si ponderas por pregunta
    }
}
