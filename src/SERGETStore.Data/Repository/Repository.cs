using DevIO.Business.Models;
using Microsoft.EntityFrameworkCore;
using SERGETStore.Business.Interfaces;
using SERGETStore.Data.Contexto;
using System.Linq.Expressions;

namespace SERGETStore.Data.Repository;


public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
{
    protected readonly SERGETStoreAppContext Db;
    readonly DbSet<TEntity> DbSet;
    protected Repository(SERGETStoreAppContext db)
    {
        Db = db;
        DbSet = db.Set<TEntity>();  
    }

    public async Task Adiciontar(TEntity entity)
    {
        DbSet.Add(entity);
        await SaveChanges();
    }

    public async Task Atualizar(TEntity entity)
    {
        DbSet.Update(entity);
        await SaveChanges();
    }

    public async Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate)
    {
         return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
    }

    public async Task<TEntity> ObterPorId(Guid id)
    {
       return await DbSet.Where(e => e.Id == id).FirstOrDefaultAsync(); 
    }

    public async Task Remover(Guid id)
    {
        
        var entity = new TEntity { Id = id };
        DbSet.Remove(entity);
        await SaveChanges();

    }

    public async Task<int> SaveChanges()
    {
        return await Db.SaveChangesAsync();
    }
    public async Task<List<TEntity>> ObterTodos()
    {
        return await DbSet.ToListAsync();
    }

    public void Dispose()
    {
        Db?.Dispose();
    }

}
