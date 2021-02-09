using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using WebAPI.Domain;

namespace WebAPI.Repository
{
    public class Context : IdentityDbContext<Usuario, Perfil, int,
                                             IdentityUserClaim<int>, PerfilUsuario, IdentityUserLogin<int>,
                                             IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public Context(DbContextOptions<Context> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PerfilUsuario>(perfilUsuario => 
            {
                perfilUsuario.HasKey(ur => new { ur.UserId, ur.RoleId });

                perfilUsuario.HasOne(ur => ur.Perfil)
                        .WithMany(r => r.PerfisUsuario)
                        .HasForeignKey(ur => ur.RoleId)
                        .IsRequired();

                perfilUsuario.HasOne(ur => ur.Usuario)
                        .WithMany(r => r.PerfisUsuario)
                        .HasForeignKey(ur => ur.UserId)
                        .IsRequired();
            });

            builder.Entity<Endereco>(org =>
            {
                org.ToTable("Enderecos");
                org.HasKey(x => x.Id);

                org.HasMany<Usuario>()
                    .WithOne()
                    .HasForeignKey(x => x.EnderecoId)
                    .IsRequired(false);
            });
        }

        public DbSet<MarcaModeloVeiculo> MarcaModeloVeiculo { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }
    }
}
