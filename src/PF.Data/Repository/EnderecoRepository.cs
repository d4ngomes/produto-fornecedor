using Microsoft.EntityFrameworkCore;
using PF.Business.Interfaces;
using PF.Business.Models;
using PF.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF.Data.Repository
{
    public class EnderecoRepository : Repository<Endereco>, IEnderecoRepository
    {
        public EnderecoRepository(AppDbContext context) : base(context) { }

        public async Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId)
        {
            return await db.Enderecos.FirstOrDefaultAsync(e => e.FornecedorId == fornecedorId);
        }
    }
}
