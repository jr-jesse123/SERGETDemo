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

    
    public async Task<Fornecedor> ObterFornecedorEndereco(Guid id)
    {
        var fornecedores_enderecos = 
            Db.Fornecedores.AsNoTracking().Include(c => c.Endereco);

        return await fornecedores_enderecos.FirstOrDefaultAsync(c => c.Id == id);
    }

    public Task<Fornecedor> ObterFornecedorProdutosEndereco(Guid id)
    {
        var output =
            Db.Fornecedores.AsNoTracking()
            .Include(f => f.Produtos)
            .Include(f => f.Endereco)
            .FirstOrDefaultAsync(f => f.Id == id);
        
        return output;
    }
}
