using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Data.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {

        public ProdutoRepository(MeuDbContext context) : base(context)
        { }

        public async Task<Produto> ObterProdutoFornecedor(Guid id)
        {
            return await DbSet.AsNoTracking().Include(f => f.Fornecedor).FirstOrDefaultAsync(p => p.Id == id);
        } //ObterProdutoFornecedor

        public async Task<IEnumerable<Produto>> ObterTodosProdutosComSeusFornecedores()
        {
            return await DbSet.AsNoTracking().Include(f => f.Fornecedor).OrderBy(p => p.Nome).ToListAsync();
        } //ObterProdutosFornecedores

        public async Task<IEnumerable<Produto>> ObterProdutosPorFornecedor(Guid fornecedorId)
        {
            return await DbSet.AsNoTracking().Where(p => p.FornecedorId == fornecedorId).OrderBy(p => p.Nome).ToListAsync();
        } //ObterProdutosPorFornecedor

    } //class

} //namespace
