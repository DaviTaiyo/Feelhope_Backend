using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Feelhope_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly string connectionString = "SuaStringDeConexaoAqui"; // Substitua com a sua string de conexão real

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto userDto)
        {
            var hashedPassword = PasswordHasher.HashPassword(userDto.Password);

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("INSERT INTO Perfil (nome, sobrenome, email, senha_hash) VALUES (@Nome, @Sobrenome, @Email, @SenhaHash)", connection);
                command.Parameters.AddWithValue("@Nome", userDto.Nome);
                command.Parameters.AddWithValue("@Sobrenome", userDto.Sobrenome);
                command.Parameters.AddWithValue("@Email", userDto.Email);
                command.Parameters.AddWithValue("@SenhaHash", hashedPassword);

                connection.Open();
                await command.ExecuteNonQueryAsync();
                connection.Close();
            }

            return Ok(new { message = "Usuário registrado com sucesso!" });
        }

        public class UserRegistrationDto
        {
            public string Nome { get; set; }
            public string Sobrenome { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}
