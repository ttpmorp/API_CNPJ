using Microsoft.EntityFrameworkCore;
using CnpjApi.Models;


namespace CnpjApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Estabelecimento> Estabelecimentos { get; set; }
        public DbSet<Simples> Simples { get; set; }
        public DbSet<Tributacao> Tributacoes { get; set; }
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