using AutoMapper;
using DevIO.App.Extensions.CustomAuthorizations;
using DevIO.App.ViewModels;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DevIO.App.Controllers
{

    [Authorize]
    public class ProdutosController : BaseController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;

        public ProdutosController(IFornecedorRepository fornecedorRepository, IProdutoRepository produtoRepository, IProdutoService produtoService, IMapper mapper, INotificadorErro notificadorErro) : base(notificadorErro)
        {
            _fornecedorRepository = fornecedorRepository ?? throw new ArgumentNullException(nameof(fornecedorRepository));
            _produtoRepository = produtoRepository ?? throw new ArgumentNullException(nameof(produtoRepository));
            _produtoService = produtoService ?? throw new ArgumentNullException(nameof(produtoService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        // GET: Produtos
        [AllowAnonymous]
        [Route("lista-de-produtos")]
        public async Task<IActionResult> Index()
        {
            var produtos = await _produtoRepository.ObterTodosProdutosComSeusFornecedores();
            var produtosVM = _mapper.Map<IEnumerable<ProdutoViewModel>>(produtos);
            return View(produtosVM);
        }

        // GET: Produtos/Details/5
        [AllowAnonymous]
        [Route("dados-do-produto/{id:Guid}")]
        public async Task<IActionResult> Details(Guid id)
        {

            var produtoVM = await ObterProdutoVM(id);

            if (produtoVM == null)
            {
                return NotFound();
            }

            return View(produtoVM);

        }

        #region === Create ========================================================================

        [ClaimsAuthorize("Produtos","Adicionar")]
        [Route("novo-produto")]
        public async Task<IActionResult> Create()
        {
            ProdutoViewModel produtoVM = await PopularFornecedores(new ProdutoViewModel());
            return View(produtoVM);
        } //Create

        // POST: Produtos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [ClaimsAuthorize("Produtos", "Adicionar")]
        [Route("novo-produto")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProdutoViewModel produtoViewModel)
        {

            if (!ModelState.IsValid)
            {
                produtoViewModel = await PopularFornecedores(produtoViewModel);
                return View(produtoViewModel);
            }

            var imgPrefixo = Guid.NewGuid() + "_";
            if (!await UploadArquivo(produtoViewModel.ImagemUpload, imgPrefixo))
            {
                produtoViewModel = await PopularFornecedores(produtoViewModel);
                return View(produtoViewModel); 
            }

            produtoViewModel.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;

            var produto = _mapper.Map<Produto>(produtoViewModel);
            await _produtoService.Adicionar(produto);

            //Verificando se algo deu errado na camada de negocios
            if(!OperacaoValida())
            {
                produtoViewModel = await PopularFornecedores(produtoViewModel);
                return View(produtoViewModel);
            }

            return RedirectToAction(nameof(Index));

        } //Create

        #endregion

        #region === Edit ==========================================================================

        // GET: Produtos/Edit/5
        [ClaimsAuthorize("Produtos", "Editar")]
        [Route("editar-produto/{id:Guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {

            var produtoViewModel = await ObterProdutoVM(id);
            if (produtoViewModel == null)
            {
                return NotFound();
            }

            produtoViewModel = await PopularFornecedores(produtoViewModel);
            
            return View(produtoViewModel);

        } //Edit

        // POST: Produtos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [ClaimsAuthorize("Produtos", "Editar")]
        [Route("editar-produto/{id:Guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProdutoViewModel produtoViewModel)
        {
            if (id != produtoViewModel.Id)
            {
                return NotFound();
            }

            //Se vou devolver a ViewModel (em caso de erro), preciso devolver o objeto Fornecedor (que eh exibido)
            var produtoOriginalViewModel = await ObterProdutoVM(id);
            produtoViewModel.Fornecedor = produtoOriginalViewModel.Fornecedor;
            produtoViewModel.Imagem = produtoOriginalViewModel.Imagem;

            if (!ModelState.IsValid)
            {
                return View(produtoViewModel);
            }

            //Validando se a imagem foi preenchida (posso nao querer atualizar)
            if (produtoViewModel.ImagemUpload != null)
            {
                var imgPrefixo = Guid.NewGuid() + "_";
                if (!await UploadArquivo(produtoViewModel.ImagemUpload, imgPrefixo))
                {
                    return View(produtoViewModel);
                }

                produtoOriginalViewModel.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;

            }

            //Copiando dados do produto recebido no formulario (que nao contem todos os campos do objeto original)
            produtoOriginalViewModel.Nome = produtoViewModel.Nome;
            produtoOriginalViewModel.Descricao = produtoViewModel.Descricao;
            produtoOriginalViewModel.Valor = produtoViewModel.Valor;
            produtoOriginalViewModel.Ativo = produtoViewModel.Ativo;

            var produto = _mapper.Map<Produto>(produtoOriginalViewModel);

            await _produtoService.Atualizar(produto);

            //Verificando se algo deu errado na camada de negocios
            if (!OperacaoValida())
            {
                produtoViewModel = await PopularFornecedores(produtoViewModel);
                return View(produtoViewModel);
            }

            return RedirectToAction(nameof(Index));
            
        } //Edit

        #endregion

        #region === Delete ========================================================================

        // GET: Produtos/Delete/5
        [ClaimsAuthorize("Produtos", "Excluir")]
        [Route("excluir-produto/{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {

            var produtoViewModel = await ObterProdutoVM(id);
            if (produtoViewModel == null)
            {
                return NotFound();
            }

            return View(produtoViewModel);

        } //Delete

        // POST: Produtos/Delete/5
        [ClaimsAuthorize("Produtos", "Excluir")]
        [Route("excluir-produto/{id:Guid}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var produtoViewModel = await ObterProdutoVM(id);

            if (produtoViewModel == null)
                return NotFound();

            await _produtoService.Remover(id);

            //Verificando se algo deu errado na camada de negocios
            if (!OperacaoValida())
            {
                return View(nameof(Delete), id);
            }

            //Setando ViewData success, eu vou mostrar (no view component) no Index que a operacao foi realizada com sucesso.
            TempData["Sucesso"] = "sucesso";

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region === Private Helpers ===============================================================

        private async Task<ProdutoViewModel> ObterProdutoVM(Guid id)
        {

            var produto = _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterProdutoFornecedor(id));
            produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
            return produto;

            //var produto = await _produtoRepository.ObterProdutoFornecedor(id);

            //var produtoVM = _mapper.Map<ProdutoViewModel>(produto);

            //Populando agora a lista de fornecedores (que sera usada para popular combo)
            //produtoVM = await PopularFornecedores(produtoVM);

            //return produtoVM;
        }

        private async Task<ProdutoViewModel> PopularFornecedores(ProdutoViewModel produtoVM)
        {
            //Populando agora a lista de fornecedores (que sera usada para popular combo)
            var fornecedores = await _fornecedorRepository.ObterTodos();
            produtoVM.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(fornecedores);

            return produtoVM;
        }

        private async Task<bool> UploadArquivo(IFormFile arquivo, string prefixo)
        {

            if (arquivo == null || arquivo.Length == 0)
                return false;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", prefixo + arquivo.FileName);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Ja existe um arquivo com este nome");
                return false;
            }

            //Using calls Dispose Method from FileStrem automatically to release NoManaged resources.
            //Note. FileStrem implement IDisposable 
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            return true;

        } //UploadArquivo

        #endregion
    }
}
