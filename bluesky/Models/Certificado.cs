using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bluesky.Models
{
    [Table("certificados")]
    public class Certificado
    {
        [Key] public int Id { get; set; }

        [Required] public int UsuarioId { get; set; }
        [ForeignKey(nameof(UsuarioId))] public virtual Usuario Usuario { get; set; }

        [Required] public int CursoId { get; set; }
        [ForeignKey(nameof(CursoId))] public virtual Curso Curso { get; set; }

        [StringLength(100)]
        public string Codigo { get; set; } // opcional: hash único

        [StringLength(300)]
        public string RutaArchivoPdf { get; set; } // ~/Certificados/cert_X.pdf

        public DateTime FechaEmision { get; set; } = DateTime.UtcNow;
        public DateTime? FechaVencimiento { get; set; }
        public bool Revocado { get; set; } = false;
    }
}
