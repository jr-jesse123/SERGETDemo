using SERGETStore.Business.Models;

namespace SERGETStore.Business.Interfaces;

public interface IFornecedorService : IDisposable
{
    Task Adicionar(Fornecedor fornecedor);
    Task Atualizar(Fornecedor fornecedor);
    Task Remover(Fornecedor fornecedor);
    Task AtualizarEndereco(Fornecedor fornecedor);
}
