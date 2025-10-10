using Microsoft.EntityFrameworkCore;
using CnpjApi.Models;

namespace CnpjApi.Data
{
    /// <summary>
    /// Contexto do banco de dados para acesso às tabelas do CNPJ
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Tabela de empresas
        /// </summary>
        public DbSet<Empresa> Empresas { get; set; }
        
        /// <summary>
        /// Tabela de estabelecimentos (matrizes e filiais)
        /// </summary>
        public DbSet<Estabelecimento> Estabelecimentos { get; set; }
        
        /// <summary>
        /// Tabela de informações do Simples Nacional
        /// </summary>
        public DbSet<Simples> Simples { get; set; }
        
        /// <summary>
        /// Tabela de informações de tributação
        /// </summary>
        public DbSet<Tributacao> Tributacoes { get; set; }
        
        /// <summary>
        /// Tabela de sócios
        /// </summary>
        public DbSet<Socio> Socios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurações das tabelas
            modelBuilder.Entity<Empresa>(entity =>
            {
                entity.HasKey(e => e.CNPJ);
                entity.Property(e => e.CNPJ).HasMaxLength(14);
                entity.Property(e => e.RAZ_SOC).HasMaxLength(200);
                entity.Property(e => e.CAP_SOCIAL).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Estabelecimento>(entity =>
            {
                entity.HasKey(e => new { e.CNPJ, e.ORDEM });
                entity.Property(e => e.CNPJ).HasMaxLength(14);
                entity.Property(e => e.CEP).HasMaxLength(8);
            });

            modelBuilder.Entity<Simples>(entity =>
            {
                entity.HasKey(e => e.CNPJ);
                entity.Property(e => e.CNPJ).HasMaxLength(14);
            });

            modelBuilder.Entity<Tributacao>(entity =>
            {
                entity.HasKey(e => new { e.CNPJ, e.ANO });
                entity.Property(e => e.CNPJ).HasMaxLength(14);
            });

            modelBuilder.Entity<Socio>(entity =>
            {
                entity.HasKey(e => e.ID_SOCIO);
                entity.Property(e => e.CNPJ).HasMaxLength(14);
                entity.Property(e => e.CNPJ_CPF).HasMaxLength(14);
            });
        }
    }
}