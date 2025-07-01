using Microsoft.EntityFrameworkCore;
using PowerOn.Models;

namespace PowerOn.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Pacote> Pacotes { get; set; }
        public DbSet<PacoteProduto> PacoteProdutos { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Familia> Familias { get; set; }
        public DbSet<Modelo> Modelos { get; set; }
        public DbSet<Revisao> Revisoes { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }

        // Tabelas relacionadas a Clientes
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<ClienteEndereco> ClienteEnderecos { get; set; }
        public DbSet<ClienteTelefone> ClienteTelefones { get; set; }

        // Tabelas de Sistema e Utilitários
        public DbSet<Sistemas> Sistemas { get; set; }
        public DbSet<Agenda> Agendas { get; set; }
        public DbSet<LogApp> LogApps { get; set; }


        // --- Configurações Adicionais do Modelo (MUITO IMPORTANTE) ---

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configura a chave primária composta para a tabela de ligação PacoteProduto.
            // Isso informa ao Entity Framework que a identidade de um PacoteProduto
            // é a combinação do ID do Pacote e do ID do Produto.
            modelBuilder.Entity<PacoteProduto>()
                .HasKey(pp => new { pp.IdPacote, pp.IdProduto });

            // Você pode adicionar outras configurações aqui no futuro, se necessário.
        }
    }
}
