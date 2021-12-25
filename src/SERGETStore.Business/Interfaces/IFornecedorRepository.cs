using DevIO.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SERGETStore.Business.Interfaces;

namespace SERGETStore.Business.Interfaces;

public interface IFornecedorRepository : IRepository<Fornecedor>    
{
    Task<Fornecedor> ObterFornecedorEndereco(Guid id);
    Task<Fornecedor> ObterFornecedorProdutosEndereco(Guid id);
}
