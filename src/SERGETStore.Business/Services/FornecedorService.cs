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

            if (!ExecutarValidação(new FornecedorValidation(),fornecedor))
            {
                return;
            }

            var documentoExistente = await fornecedorRepository.Buscar(f => f.Documento == fornecedor.Documento);

            if (documentoExistente.Any())
            {
                Notificar("Já existe um fornecedor com esete documento informado.");
                return;
            }

            await fornecedorRepository.Adiciontar(fornecedor);



        }

        public async Task Atualizar(Fornecedor fornecedor)
        {
            if (!ExecutarValidação(new FornecedorValidation(), fornecedor))
            {
                return;
            }

            var documentoExistente = await fornecedorRepository.Buscar(f => f.Documento == fornecedor.Documento && f.Id != fornecedor.Id);
            if (documentoExistente.Any())
            {
                Notificar("Já existe um fornecedor com esete documento informado.");
                return;
            }

            await fornecedorRepository.Atualizar(fornecedor);

        }

        public async Task AtualizarEndereco(Endereco fornecedor)
        {
            if (!ExecutarValidação(new EnderecoValidation(), fornecedor))
            {
                return;
            }

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
            }


        }

    }
}
