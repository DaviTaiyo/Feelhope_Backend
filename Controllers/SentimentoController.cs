using Feelhope_Backend.Data;
using Feelhope_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Feelhope_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SentimentoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SentimentoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Sentimento
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Sentimento>>> GetSentimentos()
        {
            return await _context.Sentimentos.ToListAsync();
        }

        // GET: api/Sentimento/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Sentimento>> GetSentimento(int id)
        {
            var sentimento = await _context.Sentimentos.FindAsync(id);
            if (sentimento == null)
            {
                return NotFound();
            }
            return sentimento;
        }

        // POST: api/Sentimento
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Sentimento>> PostSentimento(Sentimento sentimento)
        {
            _context.Sentimentos.Add(sentimento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSentimento), new { id = sentimento.Id }, sentimento);
        }

        // PUT: api/Sentimento/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutSentimento(int id, Sentimento sentimento)
        {
            if (id != sentimento.Id)
            {
                return BadRequest();
            }

            _context.Entry(sentimento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SentimentoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpGet("usuario/{usuarioId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Sentimento>>> GetSentimentosByUsuarioId(int usuarioId)
        {
            var sentimentos = await _context.Sentimentos
                .Where(s => s.UsuarioId == usuarioId)
                .ToListAsync();

            if (sentimentos == null || sentimentos.Count == 0)
            {
                return NotFound("Nenhum sentimento encontrado para este usuário.");
            }

            return Ok(sentimentos);
        }

        // DELETE: api/Sentimento/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteSentimento(int id)
        {
            var sentimento = await _context.Sentimentos.FindAsync(id);
            if (sentimento == null)
            {
                return NotFound();
            }

            _context.Sentimentos.Remove(sentimento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SentimentoExists(int id)
        {
            return _context.Sentimentos.Any(e => e.Id == id);
        }
    }
}
