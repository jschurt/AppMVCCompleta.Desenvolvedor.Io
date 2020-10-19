using DevIO.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> ObterProdutosPorFornecedor(Guid fornecedorId);
       
        /// <summary>
        /// Retorna uma lista de Produtos e seus fornecedores
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Produto>> ObterTodosProdutosComSeusFornecedores();
        
        /// <summary>
        /// Retorna um produto com o seu fornecedor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Produto> ObterProdutoFornecedor(Guid id);
    }
}
