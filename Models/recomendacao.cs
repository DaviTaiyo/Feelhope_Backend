using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Feelhope_Backend.Models
{
    [Table("Recomendacao")]
    public class Recomendacao
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [StringLength(255)]
        [Column("titulo")]
        public string? Titulo { get; set; }

        [StringLength(255)]
        [Column("subtitulo")]
        public string? Subtitulo { get; set; }

        [Column("descricao")]
        public string? Descricao { get; set; }

        [Column("imagem")]
        public byte[]? Imagem { get; set; } // Representa a imagem como array de bytes (VARBINARY)

        [ForeignKey("Usuario")]
        [Column("Usuario_id")]
        public int? UsuarioId { get; set; }

        // Relação com a tabela Usuario (assumindo que existe uma relação)
        public Usuario? Usuario { get; set; }
    }
}
