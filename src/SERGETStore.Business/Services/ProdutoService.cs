using SERGETStore.Business.Models;
using SERGETStore.Business.Models.Validations;

namespace SERGETStore.Business.Interfaces
{
    public class ProdutoService : BaseService, IProdutoService
    {
        public IProdutoRepository ProdutoRepository { get; }
        public ProdutoService(IProdutoRepository produtoRepository, INotificador notificador) : base(notificador)
        {
            ProdutoRepository = produtoRepository;
        }


        public async Task Adicionar(Produto produto)
        {
            if (!ExecutarValidação(new ProdutoValidation(), produto)) return;

            await ProdutoRepository.Adiciontar(produto);
        }

        public async Task Atualizar(Produto produto)
        {
            if (!ExecutarValidação(new ProdutoValidation(), produto)) return;

            await ProdutoRepository.Atualizar(produto);
        }

        public async Task Remover(Guid id)
        {
            await ProdutoRepository.Remover(id);
        }

        public void Dispose()
        {
            ProdutoRepository.Dispose();
        }
    }
}
