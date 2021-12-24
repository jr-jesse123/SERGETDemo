using DevIO.Business.Models;
using Microsoft.EntityFrameworkCore;
using SERGETStore.Business.Interfaces;
using SERGETStore.Data.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERGETStore.Data.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        readonly IQueryable<Produto> _produtosComFornecedores; 

        public ProdutoRepository(SERGETStoreAppContext db) : base(db)
        {
            _produtosComFornecedores = 
                Db.Produtos.AsNoTracking().Include(p => p.Fornecedor);
        }

        //TODO: TESTAR
        public async Task<Produto> ObterProdutoE_Fornecedor(Guid fornecedorId)
        {
            //return await Db.Produtos.AsNoTracking().Include(p => p.Fornecedor)
            //    .FirstOrDefaultAsync(p => p.Id == fornecedorId);
            return await  _produtosComFornecedores
                .FirstOrDefaultAsync(p => p.Id == fornecedorId);
        }
        //TODO: TESTAR
        public async Task<IEnumerable<Produto>> ObterProdutosFornecedores()
        {
            return await _produtosComFornecedores
                .OrderBy(p => p.Nome).ToListAsync();
        }
        //TODO: TESTAR
        public async Task<IEnumerable<Produto>> ObterProdutosPorFornecedor(Guid fornecedorId)
        {
            return await Buscar(p => p.FornecedorId == fornecedorId);
        }
    }
}
