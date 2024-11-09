using System.ComponentModel.DataAnnotations;

namespace Feelhope_Backend.DTOs
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Senha { get; set; }
    }
}
