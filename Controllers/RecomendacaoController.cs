using Feelhope_Backend.Data;
using Feelhope_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Feelhope_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecomendacaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RecomendacaoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Recomendacao
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recomendacao>>> GetRecomendacoes()
        {
            return await _context.Recomendacoes.ToListAsync();
        }

        // GET: api/Recomendacao/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Recomendacao>> GetRecomendacao(int id)
        {
            var recomendacao = await _context.Recomendacoes.FindAsync(id);
            if (recomendacao == null)
            {
                return NotFound();
            }
            return recomendacao;
        }

        // POST: api/Recomendacao
        [HttpPost]
        public async Task<ActionResult<Recomendacao>> PostRecomendacao(Recomendacao recomendacao)
        {
            _context.Recomendacoes.Add(recomendacao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRecomendacao), new { id = recomendacao.Id }, recomendacao);
        }

        // PUT: api/Recomendacao/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecomendacao(int id, Recomendacao recomendacao)
        {
            if (id != recomendacao.Id)
            {
                return BadRequest();
            }

            _context.Entry(recomendacao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecomendacaoExists(id))
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

        // DELETE: api/Recomendacao/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecomendacao(int id)
        {
            var recomendacao = await _context.Recomendacoes.FindAsync(id);
            if (recomendacao == null)
            {
                return NotFound();
            }

            _context.Recomendacoes.Remove(recomendacao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecomendacaoExists(int id)
        {
            return _context.Recomendacoes.Any(e => e.Id == id);
        }
    }
}
