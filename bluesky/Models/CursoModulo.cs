using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bluesky.Models
{
    [Table("curso_modulos")]
    public class CursoModulo
    {
        [Key] public int Id { get; set; }

        [Required] public int CursoId { get; set; }
        [ForeignKey(nameof(CursoId))] public virtual Curso Curso { get; set; }

        [Required, StringLength(150)]
        public string Titulo { get; set; }

        public int Orden { get; set; } = 1;
        public bool Activo { get; set; } = true;
    }
}
