using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Business.Models.Validations;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Business.Services
{
    public class FornecedorService : BaseService, IFornecedorService
    {


        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IEnderecoRepository _enderecoRepository;

        public FornecedorService(IFornecedorRepository fornecedorRepository, IEnderecoRepository enderecoRepository, INotificadorErro notificadorErro) : base(notificadorErro)
        {
            _fornecedorRepository = fornecedorRepository ?? throw new ArgumentNullException(nameof(fornecedorRepository));
            _enderecoRepository = enderecoRepository ?? throw new ArgumentNullException(nameof(enderecoRepository));
        }

        public async Task Adicionar(Fornecedor fornecedor)
        {
            //Validando entidade
            if (!ExecutarValidacao(new FornecedorValidation(), fornecedor) || !ExecutarValidacao(new EnderecoValidation(), fornecedor.Endereco))
                return;

            //Verificando se o documento nao existe
            var fornecedorExistente = await _fornecedorRepository.Buscar(f => f.Documento == fornecedor.Documento);
            if (fornecedorExistente != null)
            {
                NotificarErro("Ja existe um fornecedor com este documento cadastrado.");
                return;
            }

            //Cadastrando fornecedor
            await _fornecedorRepository.Adicionar(fornecedor);

        } //Adicionar

        public async Task Atualizar(Fornecedor fornecedor)
        {
            //Validando entidade
            if (!ExecutarValidacao(new FornecedorValidation(), fornecedor))
                return;

            //Verificando se o documento nao existe
            var fornecedorExistente = await _fornecedorRepository.Buscar(f => f.Documento == fornecedor.Documento && f.Id != fornecedor.Id);
            if (fornecedorExistente != null)
            {
                NotificarErro("Ja existe um fornecedor com este documento cadastrado.");
                return;
            }

            //Cadastrando fornecedor
            await _fornecedorRepository.Atualizar(fornecedor);

        } //Atualizar

        public async Task AtualizarEndereco(Endereco endereco)
        {

            //Validando entidade
            if (!ExecutarValidacao(new EnderecoValidation(), endereco))
                return;

            await _enderecoRepository.Atualizar(endereco);

        } //AtualizarEndereco

        public async Task Remover(Guid id)
        {
            //Verificando se o fornecedor nao possui produtos cadasatrados
            var produtosFornecedor = (await _fornecedorRepository.ObterFornecedorProdutosEndereco(id)).Produtos;
            if (produtosFornecedor != null)
            {
                NotificarErro("O fornecedor possui produtos cadastrados!");
                return;
            }

            await _fornecedorRepository.Remover(id);

        } //Remover


        public void Dispose()
        {
            _fornecedorRepository?.Dispose();
            _enderecoRepository?.Dispose();
        } //Dispose


    } //class

} //namespace
