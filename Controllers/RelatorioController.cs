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

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<Relatorio>>> GetRelatoriosByUsuarioId(int usuarioId)
        {
            var relatorios = await _context.Relatorios
                                            .Where(r => r.UsuarioId == usuarioId)
                                            .ToListAsync();

            if (!relatorios.Any())
            {
                return Ok(new List<Relatorio>());
            }

            return relatorios;
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
        // POST: api/Relatorio
        [HttpPost]
        public async Task<ActionResult<Relatorio>> PostRelatorio(Relatorio relatorio)
        {
            // Verifica se o sentimento já existe na tabela Sentimento, com base no título do sentimento
            var sentimentoExistente = await _context.Sentimentos
                                                    .FirstOrDefaultAsync(s => s.Titulo.ToLower() == relatorio.Sentimentos.ToLower());

            if (sentimentoExistente != null)
            {
                // Se o sentimento já existe, atualiza o nível e o número dele na tabela Sentimento
                sentimentoExistente.Nivel += relatorio.Nivel ?? 0; // Atualiza o nível
                sentimentoExistente.Numero = (sentimentoExistente.Numero ?? 0) + (relatorio.Nivel ?? 0); // Atualiza o número

                // Define o ID do sentimento existente no relatório
                relatorio.SentimentosId = sentimentoExistente.Id;

                // Atribui o ID do usuário ao sentimento se ainda não estiver definido
                if (sentimentoExistente.UsuarioId == null)
                {
                    sentimentoExistente.UsuarioId = relatorio.UsuarioId;
                }

                // Marca o sentimento como modificado no banco de dados
                _context.Entry(sentimentoExistente).State = EntityState.Modified;
            }
            else
            {
                // Se o sentimento não existe, cria um novo
                var novoSentimento = new Sentimento
                {
                    Titulo = relatorio.Sentimentos,
                    Nivel = relatorio.Nivel ?? 0, // Define o nível com o valor do relatório
                    Numero = relatorio.Nivel ?? 0, // Define o número inicial igual ao nível
                    Descricao = "", // Deixa a descrição vazia conforme solicitado
                    UsuarioId = relatorio.UsuarioId // Define o ID do usuário no novo sentimento
                };

                // Adiciona o novo sentimento ao contexto e salva para gerar o ID
                _context.Sentimentos.Add(novoSentimento);
                await _context.SaveChangesAsync();

                // Define o ID do novo sentimento no relatório
                relatorio.SentimentosId = novoSentimento.Id;
                relatorio.Sentimento = novoSentimento; // Associa o novo sentimento ao relatório
            }

            // Adiciona o relatório à tabela Relatorios e salva
            _context.Relatorios.Add(relatorio);
            await _context.SaveChangesAsync();

            // Inclui o objeto `Sentimento` associado no retorno para manter o formato de resposta desejado
            relatorio.Sentimento = await _context.Sentimentos
                                                .Where(s => s.Id == relatorio.SentimentosId)
                                                .FirstOrDefaultAsync();

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
