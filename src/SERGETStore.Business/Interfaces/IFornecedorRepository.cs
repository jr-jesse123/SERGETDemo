using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SERGETStore.Business.Interfaces;
using SERGETStore.Business.Models;

namespace SERGETStore.Business.Interfaces;

public interface IFornecedorRepository : IRepository<Fornecedor>    
{
    Task<Fornecedor> ObterFornecedorEnderecoPorId(Guid id);
    Task<Fornecedor> ObterFornecedorProdutosEnderecoPorId(Guid id);
}
