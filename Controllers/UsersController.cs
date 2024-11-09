using Feelhope_Backend.Data;
using Feelhope_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Feelhope_Backend.DTOs;
using System.Threading.Tasks;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly JwtTokenGenerator _tokenGenerator;

    public UsuarioController(AppDbContext context, JwtTokenGenerator? tokenGenerator)
    {
        _context = context;
        _tokenGenerator = tokenGenerator;
    }

    // Criar (Registrar) um novo usuário
    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar([FromBody] Usuario usuario)
    {
        // Verificar se o email, telefone ou CPF já existem
        if (_context.Usuarios.Any(u => u.Email == usuario.Email || u.Telefone == usuario.Telefone || u.CPF == usuario.CPF))
            return BadRequest("Email, Telefone ou CPF já registrado!");

        // Hashear a senha antes de armazenar
        usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);

        // Adicionar o novo usuário ao banco
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return Ok("Usuário registrado com sucesso.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Senha, usuario.Senha))
            return Unauthorized("Usuário ou senha inválidos.");

        var token = _tokenGenerator.GenerateToken(usuario);

        // Inclui o CRM no retorno, assumindo que o usuário tenha um campo 'CRM' disponível
        return Ok(new { Token = token, CRM = usuario.CRM });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get user id from token

        if (userId == null)
        {
            return Unauthorized("Token inválido.");
        }

        var usuario = await _context.Usuarios
            .Where(u => u.Id.ToString() == userId)
            .Select(u => new
            {
                u.Id,
                u.Nome,
                u.Sobrenome,
                u.Email,
                u.DataNascimento,
                u.Telefone,
                u.CPF,
                u.NomeClinica,
                u.CRM
            })
            .FirstOrDefaultAsync();

        if (usuario == null)
        {
            return NotFound("Usuário não encontrado.");
        }

        return Ok(usuario);
    }

// Atualizar perfil do usuário
[HttpPut("atualizar/{id}")]
    [Authorize]
    public async Task<IActionResult> Atualizar(int id, [FromBody] Usuario dadosAtualizados)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
            return NotFound("Usuário não encontrado.");

        usuario.Nome = dadosAtualizados.Nome;
        usuario.Sobrenome = dadosAtualizados.Sobrenome;
        usuario.Email = dadosAtualizados.Email;
        usuario.Telefone = dadosAtualizados.Telefone;
        usuario.CPF = dadosAtualizados.CPF;
        usuario.DataNascimento = dadosAtualizados.DataNascimento;
        usuario.CRM = dadosAtualizados.CRM;
        usuario.NomeClinica = dadosAtualizados.NomeClinica;

        await _context.SaveChangesAsync();
        return Ok("Perfil atualizado.");
    }

    // Atualizar a senha do usuário
    [HttpPut("atualizar-senha/{id}")]
    [Authorize]
    public async Task<IActionResult> AtualizarSenha(int id, [FromBody] string novaSenha)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
            return NotFound("Usuário não encontrado.");

        usuario.Senha = BCrypt.Net.BCrypt.HashPassword(novaSenha);
        await _context.SaveChangesAsync();
        return Ok("Senha atualizada.");
    }

    // Deletar usuário
    [HttpDelete("deletar/{id}")]
    [Authorize]
    public async Task<IActionResult> Deletar(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
            return NotFound("Usuário não encontrado.");

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
        return Ok("Usuário deletado.");
    }

    // Listar todos os usuários
    [HttpGet("listar")]
    [Authorize]
    public async Task<IActionResult> GetUsuarios()
    {
        var usuarios = await _context.Usuarios.ToListAsync();
        return Ok(usuarios);
    }

    // Back-end: Endpoint para retornar relatórios por usuário
    [HttpGet("usuario/{usuarioId}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Relatorio>>> GetRelatoriosByUsuarioId(int usuarioId)
    {
        var relatorios = await _context.Relatorios
                                       .Where(r => r.UsuarioId == usuarioId)
                                       .ToListAsync();
        return Ok(relatorios);
    }

}
