using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Feelhope_Backend.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [StringLength(255)]
        [Column("nome")]
        public string? Nome { get; set; }

        [StringLength(255)]
        [Column("sobrenome")]
        public string? Sobrenome { get; set; }

        [StringLength(255)]
        [Column("email")]
        [EmailAddress]
        public string? Email { get; set; }

        [DataType(DataType.Date)]
        [Column("data_nascimento")]
        public DateTime? DataNascimento { get; set; }

        [StringLength(20)]
        [Column("telefone")]
        public string? Telefone { get; set; }

        [StringLength(14)]
        [Column("cpf")]
        public string? CPF { get; set; }

        [StringLength(255)]
        [Column("nome_clinica")]
        public string? NomeClinica { get; set; }

        [StringLength(50)]
        [Column("crm")]
        public string? CRM { get; set; }

        [Column("foto")]
        public byte[]? Foto { get; set; }

        [StringLength(255)]
        [Column("senha_hash")]
        public string? Senha { get; set; }
    }
}
