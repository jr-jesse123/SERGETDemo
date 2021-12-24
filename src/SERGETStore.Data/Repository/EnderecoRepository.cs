using DevIO.Business.Models;
using Microsoft.EntityFrameworkCore;
using SERGETStore.Business.Interfaces;
using SERGETStore.Data.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
