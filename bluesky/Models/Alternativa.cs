using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bluesky.Models
{
    [Table("alternativas")]
    public class Alternativa
    {
        [Key] public int Id { get; set; }

        [Required] public int PreguntaId { get; set; }
        [ForeignKey(nameof(PreguntaId))] public virtual Pregunta Pregunta { get; set; }

        [Required, StringLength(800)]
        public string Texto { get; set; }

        public bool EsCorrecta { get; set; } = false;
        public int Orden { get; set; } = 1;
        public bool Activa { get; set; } = true;
    }
}
