using DevIO.Business.Models;
using Microsoft.EntityFrameworkCore;
using SERGETStore.Data.Extentions;

namespace SERGETStore.Data.Contexto;
public class SERGETStoreAppContext : DbContext
{
    public SERGETStoreAppContext(DbContextOptions<SERGETStoreAppContext> options) : base(options)    {    }

    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }
    public DbSet<Fornecedor> Fornecedores { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.DesabilitarCascadingDelete();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SERGETStoreAppContext).Assembly);



        base.OnModelCreating(modelBuilder);
    }
}
