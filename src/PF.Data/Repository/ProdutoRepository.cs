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
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context) { }

        public async Task<Produto> ObterProdutoFornecedor(Guid id)
        {
            return await db.Produtos.Include(m => m.Fornecedor).SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Produto>> ObterProdutosFornecedores()
        {
            return await db.Produtos.Include(p => p.Fornecedor).OrderBy(p => p.Nome).ToListAsync();
        }

        public async Task<IEnumerable<Produto>> ObterProdutosPorFornecedor(Guid fornecedorId)
        {
            return await db.Produtos.Include(m => m.Fornecedor).Where(m => m.FornecedorId == fornecedorId).ToListAsync();
        }
    }
}
