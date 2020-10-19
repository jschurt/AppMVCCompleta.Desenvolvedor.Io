using DevIO.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    public interface IFornecedorRepository: IRepository<Fornecedor>
    {
        /// <summary>
        /// Retorna o endereco de um dado fornecedor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Fornecedor> ObterFornecedorEndereco(Guid id);

        /// <summary>
        /// Retorna Fornecedor + Lista de Produtos + Endereco do Fornecedor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Fornecedor> ObterFornecedorProdutosEndereco(Guid id);

    } //interface
} //namespace
