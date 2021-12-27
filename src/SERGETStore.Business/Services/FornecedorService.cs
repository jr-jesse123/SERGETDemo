using SERGETStore.Business.Models;
using SERGETStore.Business.Models.Validations;

namespace SERGETStore.Business.Interfaces
{
    public class FornecedorService : BaseService, IFornecedorService
    {
        public async Task Adicionar(Fornecedor fornecedor)
        {
            //validar estado
            var validator = new FornecedorValidation();
            var result = validator.Validate(fornecedor);

            if (!ExecutarValidação(new FornecedorValidation(),fornecedor))
            {
                return;
            }


        }

        public async Task Atualizar(Fornecedor fornecedor)
        {
            if (!ExecutarValidação(new FornecedorValidation(), fornecedor))
            {
                return;
            }

        }

        public async Task AtualizarEndereco(Fornecedor fornecedor)
        {
            if (!ExecutarValidação(new FornecedorValidation(), fornecedor))
            {
                return;
            }

        }

        public async Task Remover(Fornecedor fornecedor)
        {
            if (!ExecutarValidação(new FornecedorValidation(), fornecedor))
            {
                return;
            }

        }
    }
}
