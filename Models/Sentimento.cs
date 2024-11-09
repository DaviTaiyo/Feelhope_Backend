using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Feelhope_Backend.Models
{
    [Table("Sentimentos")]
    public class Sentimento
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [StringLength(255)]
        [Column("titulo")]
        public string? Titulo { get; set; }

        [Column("nivel")]
        public int? Nivel { get; set; }

        [Column("descricao")]
        public string? Descricao { get; set; }

        [Column("numero")]
        public int? Numero { get; set; }

        [ForeignKey("Usuario")]
        [Column("Usuario_id")]
        public int? UsuarioId { get; set; }

        // Relação com a tabela Usuario (supondo que existe um relacionamento)
        public Usuario? Usuario { get; set; }
    }
}
