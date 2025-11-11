using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bluesky.Models
{
    [Table("archivos")]
    public class Archivo
    {
        [Key] public int Id { get; set; }

        [Required, StringLength(200)]
        public string NombreArchivo { get; set; }

        [Required, StringLength(300)]
        public string Ruta { get; set; } // ~/Uploads/...

        [StringLength(120)]
        public string ContentType { get; set; }

        public long PesoBytes { get; set; }

        public DateTime FechaSubida { get; set; } = DateTime.UtcNow;

        public int? UsuarioId { get; set; }
        [ForeignKey(nameof(UsuarioId))] public virtual Usuario Usuario { get; set; }
    }
}
