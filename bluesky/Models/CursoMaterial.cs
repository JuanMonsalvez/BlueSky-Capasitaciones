using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bluesky.Models
{
    public enum TipoMaterial { Pdf = 1, Video = 2, Link = 3, Imagen = 4, Otro = 9 }

    [Table("curso_materiales")]
    public class CursoMaterial
    {
        [Key] public int Id { get; set; }

        [Required] public int CursoId { get; set; }
        [ForeignKey(nameof(CursoId))] public virtual Curso Curso { get; set; }

        public int? ModuloId { get; set; }
        [ForeignKey(nameof(ModuloId))] public virtual CursoModulo Modulo { get; set; }

        [Required, StringLength(200)]
        public string NombreArchivo { get; set; }

        [Required, StringLength(300)]
        public string RutaAlmacenamiento { get; set; } // ~/Uploads/...

        public TipoMaterial Tipo { get; set; } = TipoMaterial.Pdf;

        public DateTime FechaSubida { get; set; } = DateTime.UtcNow;
        public bool Activo { get; set; } = true;
    }
}
