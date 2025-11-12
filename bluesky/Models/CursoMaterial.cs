using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bluesky.Models
{
    public enum TipoMaterial
    {
        Pdf = 1,
        Ppt = 2,
        Otro = 3
    }

    [Table("curso_materiales")]
    public class CursoMaterial
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CursoId { get; set; }
        [ForeignKey(nameof(CursoId))]
        public virtual Curso Curso { get; set; }

        public int? ModuloId { get; set; }
        [ForeignKey(nameof(ModuloId))]
        public virtual CursoModulo Modulo { get; set; }

        [Required, StringLength(200)]
        public string NombreArchivo { get; set; }

        [Required, StringLength(300)]
        public string RutaAlmacenamiento { get; set; }

        [Required]
        public TipoMaterial Tipo { get; set; } = TipoMaterial.Otro;

        [Required]
        public DateTime FechaSubida { get; set; } = DateTime.UtcNow;

        public bool Activo { get; set; } = true;
    }
}
