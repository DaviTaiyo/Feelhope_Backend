using Feelhope_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Feelhope_Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Relatorio> Relatorios { get; set; }
        public DbSet<Sentimento> Sentimentos { get; set; }
        public DbSet<Recomendacao> Recomendacoes { get; set; }
    }
}
