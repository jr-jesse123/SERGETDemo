using SERGETStore.Business.Models;

namespace SERGETStore.Business.Interfaces;

internal interface IProdutoService
{
    Task Adicionar(Produto produto);
    Task Atualizar(Produto produto);
    Task Remover(Guid guid);
}
