using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Feelhope_Backend.Models
{
    [Table("Relatorios")]
    public class Relatorio
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [StringLength(255)]
        [Column("sentimentos")]
        public string? Sentimentos { get; set; }

        [Column("nivel")]
        public int? Nivel { get; set; }

        [Column("descricao_relatorio")]
        public string? DescricaoRelatorio { get; set; }

        [Column("audio")]
        public byte[]? Audio { get; set; } // Representa o áudio como array de bytes (VARBINARY)

        [ForeignKey("Sentimento")]
        [Column("sentimentos_id")]
        public int? SentimentosId { get; set; }

        [ForeignKey("Usuario")]
        [Column("Usuario_id")]
        public int? UsuarioId { get; set; }

        // Relações (opcionais, dependendo das necessidades do projeto)
        public Sentimento? Sentimento { get; set; }
        public Usuario? Usuario { get; set; }
    }
}
