using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Business.Models.Validations;
using System;
using System.Threading.Tasks;

namespace DevIO.Business.Services
{
    public class ProdutoService : BaseService, IProdutoService
    {

        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository, INotificadorErro notificadorErro) : base(notificadorErro)
        {
            _produtoRepository = produtoRepository ?? throw new ArgumentNullException(nameof(produtoRepository));
        }

        public async Task Adicionar(Produto produto)
        {
            //Validando entidade
            if (!ExecutarValidacao(new ProdutoValidation(), produto))
                return;

            await _produtoRepository.Adicionar(produto);

        } //Adicionar

        public async Task Atualizar(Produto produto)
        {
            //Validando entidade
            if (!ExecutarValidacao(new ProdutoValidation(), produto))
                return;

            await _produtoRepository.Atualizar(produto);

        } //Atualizar

        public async Task Remover(Guid id)
        {
            await _produtoRepository.Remover(id);
        } //Remove

        public void Dispose()
        {
            _produtoRepository?.Dispose();
        }


    } //class

} //namespace
