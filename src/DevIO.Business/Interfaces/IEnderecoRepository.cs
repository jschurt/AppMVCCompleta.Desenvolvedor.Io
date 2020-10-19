using DevIO.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    public interface IEnderecoRepository: IRepository<Endereco>
    {
        /// <summary>
        /// Retorna o endereco de um fornecedor
        /// </summary>
        /// <param name="fornecedorId"></param>
        /// <returns></returns>
        Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId);    
    }
    
}
