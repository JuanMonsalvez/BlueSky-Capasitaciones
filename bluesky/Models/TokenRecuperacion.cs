using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bluesky.Models
{
    [Table("tokens_recuperacion")]
    public class TokenRecuperacion
    {
        [Key] public int Id { get; set; }

        [Required] public int UsuarioId { get; set; }
        [ForeignKey(nameof(UsuarioId))] public virtual Usuario Usuario { get; set; }

        [Required, StringLength(100)]
        public string Token { get; set; }

        public DateTime ExpiraEn { get; set; }
        public bool Utilizado { get; set; } = false;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }
}
