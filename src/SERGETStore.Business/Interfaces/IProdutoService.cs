using SERGETStore.Business.Models;

namespace SERGETStore.Business.Interfaces;

public interface IProdutoService : IDisposable
{
    Task Adicionar(Produto produto);
    Task Atualizar(Produto produto);
    Task Remover(Guid guid);
}
