using Feelhope_Backend.Data;
using Feelhope_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Feelhope_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelatorioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RelatorioController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Relatorio
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Relatorio>>> GetRelatorios()
        {
            return await _context.Relatorios.ToListAsync();
        }

        // GET: api/Relatorio/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Relatorio>> GetRelatorio(int id)
        {
            var relatorio = await _context.Relatorios.FindAsync(id);
            if (relatorio == null)
            {
                return NotFound();
            }
            return relatorio;
        }

        // POST: api/Relatorio
        [HttpPost]
        public async Task<ActionResult<Relatorio>> PostRelatorio(Relatorio relatorio)
        {
            _context.Relatorios.Add(relatorio);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRelatorio), new { id = relatorio.Id }, relatorio);
        }

        // PUT: api/Relatorio/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRelatorio(int id, Relatorio relatorio)
        {
            if (id != relatorio.Id)
            {
                return BadRequest();
            }

            _context.Entry(relatorio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RelatorioExists(id))
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

        // DELETE: api/Relatorio/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRelatorio(int id)
        {
            var relatorio = await _context.Relatorios.FindAsync(id);
            if (relatorio == null)
            {
                return NotFound();
            }

            _context.Relatorios.Remove(relatorio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RelatorioExists(int id)
        {
            return _context.Relatorios.Any(e => e.Id == id);
        }
    }
}
