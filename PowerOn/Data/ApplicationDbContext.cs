using Microsoft.EntityFrameworkCore;
using PowerOn.Models; // Garante que o DbContext enxergue todas as suas classes de modelo

namespace PowerOn.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // --- Mapeamento das Tabelas para DbSets ---

        // Tabelas Principais e de Ligação
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Pacote> Pacotes { get; set; }
        public DbSet<PacoteProduto> PacoteProdutos { get; set; } // Tabela de ligação

        // Tabelas relacionadas a Veículos
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Familia> Familias { get; set; }
        public DbSet<Modelo> Modelos { get; set; }
        public DbSet<Revisao> Revisoes { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; } // Nome corrigido de Viculo

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
