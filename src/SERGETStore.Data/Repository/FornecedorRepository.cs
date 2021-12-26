using DevIO.Business.Models;
using Microsoft.EntityFrameworkCore;
using SERGETStore.Business.Interfaces;
using SERGETStore.Data.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERGETStore.Data.Repository;

public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
{
    
    public FornecedorRepository(SERGETStoreAppContext db) : base(db)
    {
    }

    public override async Task<List<Fornecedor>> ObterTodos()
    {
        return await Db.Fornecedores.AsNoTracking().Include(f => f.Endereco).ToListAsync();
    }

    //TODO: VERIFICAR A ESPECIALIZAÇÃO DESTES DOIS MÉDOTOS, TEM CARA DE LEAK AB
    public override async Task<Fornecedor> ObterPorId(Guid id)
    {
        return await Db.Fornecedores.Include(f => f.Endereco).Where(e => e.Id == id).FirstOrDefaultAsync();
    }


    public async Task<Fornecedor> ObterFornecedorEnderecoPorId(Guid id)
    {
        var fornecedores_enderecos = 
            Db.Fornecedores.AsNoTracking().Include(c => c.Endereco);

        return await fornecedores_enderecos.FirstOrDefaultAsync(c => c.Id == id);
    }
    /// <summary>
    /// Retorna O primeiro Fornecedor com a id do paramêtro
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<Fornecedor> ObterFornecedorProdutosEnderecoPorId(Guid id)
    {
        var output =
            Db.Fornecedores.AsNoTracking()
            .Include(f => f.Produtos)
            .Include(f => f.Endereco)
            .FirstOrDefaultAsync(f => f.Id == id);
        
        return output;
    }
}
