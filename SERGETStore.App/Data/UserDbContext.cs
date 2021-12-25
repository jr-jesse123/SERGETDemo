using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SERGETStore.App.ViewModels;

namespace SERGETStore.App.Data
{
    public class UserDbContext : IdentityDbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }
        public DbSet<SERGETStore.App.ViewModels.ProdutoViewModel> ProdutoViewModel { get; set; }
        
    }
}