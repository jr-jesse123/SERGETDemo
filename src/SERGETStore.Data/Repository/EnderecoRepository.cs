using Microsoft.EntityFrameworkCore;
using SERGETStore.Business.Interfaces;
using SERGETStore.Business.Models;
using SERGETStore.Data.Contexto;

namespace SERGETStore.Data.Repository;

public class EnderecoRepository : Repository<Endereco>, IEnderecoRepository
{
    public EnderecoRepository(SERGETStoreAppContext db) : base(db)
    {
    }

    //TODO: TESTAR
    public async Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId)
    {
        return await Db.Enderecos.AsNoTracking()
            .FirstOrDefaultAsync(f => f.FornecedorId == fornecedorId);
    }
}
