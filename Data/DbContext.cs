using Microsoft.EntityFrameworkCore;
using ProjetoBackend.Models;

namespace ProjetoBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Representa a tabela 'documentos' no banco de dados
        public DbSet<Usuario> Usuarios { get; set; }
    }
}