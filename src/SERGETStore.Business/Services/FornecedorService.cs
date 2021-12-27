using SERGETStore.Business.Models;
using SERGETStore.Business.Models.Validations;

namespace SERGETStore.Business.Interfaces
{
    public class FornecedorService : BaseService, IFornecedorService
    {

        private readonly IFornecedorRepository fornecedorRepository;
        private readonly IEnderecoRepository enderecoRepository;
        private readonly INotificador notificador;

        public FornecedorService(IFornecedorRepository fornecedorRepository, 
                                    IEnderecoRepository enderecoRepository, 
                                    INotificador notificador) : base(notificador)
        {
            this.fornecedorRepository = fornecedorRepository;
            this.enderecoRepository = enderecoRepository;
            this.notificador = notificador;
        }

        public async Task Adicionar(Fornecedor fornecedor)
        {
            //validar estado
            var validator = new FornecedorValidation();
            var result = validator.Validate(fornecedor);

            if (!ExecutarValidação(new FornecedorValidation(),fornecedor) 
                || !ExecutarValidação(new EnderecoValidation(), fornecedor.Endereco)) 
                return;
            

            var documentosIguais = await fornecedorRepository.Buscar(f => f.Documento == fornecedor.Documento);

            if (documentosIguais.Any())
            {
                Notificar("Já existe um fornecedor com esete documento informado.");
                return;
            }

            await fornecedorRepository.Adiciontar(fornecedor);
        }

        public async Task Atualizar(Fornecedor fornecedor)
        {
            if (!ExecutarValidação(new FornecedorValidation(), fornecedor))
                return;
            
            var documentoIgual = await fornecedorRepository.Buscar(f => f.Documento == fornecedor.Documento && f.Id != fornecedor.Id);

            if (documentoIgual.Any())
            {
                Notificar("Já existe um fornecedor com esete documento informado.");
                return;
            }

            await fornecedorRepository.Atualizar(fornecedor);
        }

        public async Task AtualizarEndereco(Endereco endereco)
        {
            if (!ExecutarValidação(new EnderecoValidation(), endereco))
                return;
            
            await enderecoRepository.Atualizar(endereco);
        }

        public void Dispose()
        {
            fornecedorRepository?.Dispose();
        }

        public async Task Remover(Guid id)
        {
            var fornecedorE_Produtos = await fornecedorRepository.ObterFornecedorProdutosEnderecoPorId(id);

            if (fornecedorE_Produtos.Produtos.Any())
            {
                Notificar("O fornecedor possui produtos cadastrados!");
                return ;
            }

            //remove endereços
            var endereco = await enderecoRepository.ObterEnderecoPorFornecedor(id);

            if (endereco != null)
                await enderecoRepository.Remover(endereco.Id);
            
            //remove fornecedor
            await fornecedorRepository.Remover(id);
        }
    }
}
