using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bluesky.Models
{
    [Table("curso_lecciones")]
    public class CursoLeccion
    {
        [Key] public int Id { get; set; }

        [Required] public int ModuloId { get; set; }
        [ForeignKey(nameof(ModuloId))] public virtual CursoModulo Modulo { get; set; }

        [Required, StringLength(150)]
        public string Titulo { get; set; }

        [StringLength(4000)]
        public string ContenidoHtml { get; set; } // o Markdown si prefieres

        public int Orden { get; set; } = 1;
        public bool Activo { get; set; } = true;
    }
}
