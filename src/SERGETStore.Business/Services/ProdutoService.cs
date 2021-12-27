using SERGETStore.Business.Models;

namespace SERGETStore.Business.Interfaces
{
    public class ProdutoService : BaseService, IProdutoService
    {
        public Task Adicionar(Produto produto)
        {
            throw new NotImplementedException();
        }

        public Task Atualizar(Produto produto)
        {
            throw new NotImplementedException();
        }

        public Task Remover(Guid guid)
        {
            throw new NotImplementedException();
        }
    }
}
