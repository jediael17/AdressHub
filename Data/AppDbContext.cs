using AddressManager.Models;
using Microsoft.EntityFrameworkCore;

namespace AddressManager.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Endereco> Enderecos => Set<Endereco>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuarios");
                entity.HasIndex(u => u.Usuario1).IsUnique();
                entity.Property(u => u.Usuario1).HasColumnName("usuario");
                entity.Property(u => u.Id).HasColumnName("id");
                entity.Property(u => u.Nome).HasColumnName("nome");
                entity.Property(u => u.Senha).HasColumnName("senha");
                entity.Property(u => u.DataCriacao).HasColumnName("datacriacao");
            });

            modelBuilder.Entity<Endereco>(entity =>
            {
                entity.ToTable("enderecos");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Cep).HasColumnName("cep").HasMaxLength(8);
                entity.Property(e => e.Logradouro).HasColumnName("logradouro");
                entity.Property(e => e.Complemento).HasColumnName("complemento");
                entity.Property(e => e.Bairro).HasColumnName("bairro");
                entity.Property(e => e.Cidade).HasColumnName("cidade");
                entity.Property(e => e.Uf).HasColumnName("uf").HasMaxLength(2);
                entity.Property(e => e.Numero).HasColumnName("numero");
                entity.Property(e => e.UsuarioId).HasColumnName("usuarioid");
                entity.Property(e => e.DataCriacao).HasColumnName("datacriacao");
                entity.Property(e => e.DataAtualizacao).HasColumnName("dataatualizacao");

                entity.HasOne(e => e.Usuario)
                      .WithMany(u => u.Enderecos)
                      .HasForeignKey(e => e.UsuarioId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.UsuarioId);
            });
        }
    }
}
