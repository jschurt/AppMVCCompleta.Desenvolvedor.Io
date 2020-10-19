using AutoMapper;
using DevIO.App.Extensions.CustomAuthorizations;
using DevIO.App.ViewModels;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevIO.App.Controllers
{

    [Authorize]
    public class FornecedoresController : BaseController
    {

        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IFornecedorService _fornecedorService;
        private readonly IMapper _mapper;

        public FornecedoresController(IFornecedorRepository fornecedorRepository, IFornecedorService fornecedorService, IMapper mapper, INotificadorErro notificadorErro) : base(notificadorErro)
        {
            _fornecedorRepository = fornecedorRepository ?? throw new ArgumentNullException(nameof(fornecedorRepository));
            _fornecedorService = fornecedorService ?? throw new ArgumentNullException(nameof(fornecedorService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // GET: Fornecedores
        [AllowAnonymous]
        [Route("lista-de-fornecedores")]
        public async Task<IActionResult> Index()
        {
            var fornecedores = await _fornecedorRepository.ObterTodos();
            var fornecedoresVM = _mapper.Map<IEnumerable<FornecedorViewModel>>(fornecedores);

            return View(fornecedoresVM);
        } //Index

        // GET: Fornecedores/Details/5
        [AllowAnonymous]
        [Route("dados-do-fornecedor/{id:Guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorEndereco(id);
            
            if (fornecedorViewModel == null)
            {
                return NotFound();
            }

            return View(fornecedorViewModel);
        }

        #region === Create ========================================================================

        // GET: Fornecedores/Create
        [ClaimsAuthorize("Fornecedores", "Adicionar")]
        [Route("novo-fornecedor")]
        public IActionResult Create()
        {
            return View();
        } //Create

        // POST: Fornecedores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [ClaimsAuthorize("Fornecedores", "Adicionar")]
        [Route("novo-fornecedor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FornecedorViewModel fornecedorViewModel)
        {
            if (!ModelState.IsValid)
                return View(fornecedorViewModel);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
            await _fornecedorService.Adicionar(fornecedor);

            //Verificando se algo deu errado na camada de negocios
            if (!OperacaoValida())
            {
                return View(fornecedorViewModel);
            }


            return RedirectToAction(nameof(Index));
            
        } //Create

        #endregion

        #region === Edit ==========================================================================

        // GET: Fornecedores/Edit/5
        [ClaimsAuthorize("Fornecedores", "Editar")]
        [Route("editar-fornecedor/{id:Guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {

            var fornecedorViewModel = await ObterFornecedorProdutosEndereco(id);

            if (fornecedorViewModel == null)
            {
                return NotFound();
            }

            return View(fornecedorViewModel);

        } //Edit

        // POST: Fornecedores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [ClaimsAuthorize("Fornecedores", "Editar")]
        [Route("editar-fornecedor/{id:Guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, FornecedorViewModel fornecedorViewModel)
        {
            if (id != fornecedorViewModel.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return View(fornecedorViewModel);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
            await _fornecedorService.Atualizar(fornecedor);

            //Verificando se algo deu errado na camada de negocios
            if (!OperacaoValida())
            {
                return View(fornecedorViewModel);
            }

            return RedirectToAction(nameof(Index));
            
        } //Edit

        #endregion

        #region === Delete ========================================================================

        // GET: Fornecedores/Delete/5
        [ClaimsAuthorize("Fornecedores", "Excluir")]
        [Route("excluir-fornecedor/{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {


            var fornecedorVM = await ObterFornecedorProdutosEndereco(id);


            if (fornecedorVM == null)
            {
                return NotFound();
            }

            return View(fornecedorVM);
        } //Delete

        // POST: Fornecedores/Delete/5
        [ClaimsAuthorize("Fornecedores", "Excluir")]
        [Route("excluir-fornecedor/{id:Guid}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {

            var fornecedorVM = await ObterFornecedorEndereco(id);
            if (fornecedorVM == null)
                return NotFound();

            await _fornecedorService.Remover(id);
            //Verificando se algo deu errado na camada de negocios
            if (!OperacaoValida())
            {
                return View(id);
            }

            return RedirectToAction(nameof(Index));
        
        } //DeleteConfirmed

        #endregion

        #region === Partial View AtualizarEndereco ================================================

        /// <summary>
        /// Carrega Partial View DetalhesEndereco
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("obter-endereco-fornecedor/{id:Guid}")]
        public async Task<IActionResult> ObterEndereco(Guid id)
        {
            var fornecedor = await ObterFornecedorEndereco(id);

            if (fornecedor == null)
            {
                return NotFound();
            }

            return PartialView("_DetalhesEndereco", fornecedor);
        }

        /// <summary>
        /// Partial View para edicao de endereco 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ClaimsAuthorize("Fornecedores", "Editar")]
        [Route("atualizar-endereco-fornecedor/{id:Guid}")]
        public async Task<IActionResult> AtualizarEndereco(Guid id)
        {
            var fornecedor = await ObterFornecedorEndereco(id);

            if (fornecedor == null)
            {
                return NotFound();
            }

            return PartialView("_AtualizarEndereco", new FornecedorViewModel { Endereco = fornecedor.Endereco });
        }

        [ClaimsAuthorize("Fornecedores", "Editar")]
        [Route("atualizar-endereco-fornecedor/{id:Guid}")]
        [HttpPost]
        public async Task<IActionResult> AtualizarEndereco(FornecedorViewModel fornecedorViewModel)
        {
            //Como estou validando FornecedorViewModel, devo remover as validacoes especificas do fornecedor
            ModelState.Remove("Nome");
            ModelState.Remove("Documento");

            if (!ModelState.IsValid) return PartialView("_AtualizarEndereco", fornecedorViewModel);

            await _fornecedorService.AtualizarEndereco(_mapper.Map<Endereco>(fornecedorViewModel.Endereco));

            if (!OperacaoValida()) return PartialView("_AtualizarEndereco", fornecedorViewModel);

            //Retornando url para a action ObterEndereco
            var url = Url.Action("ObterEndereco", "Fornecedores", new { id = fornecedorViewModel.Endereco.FornecedorId });
            return Json(new { success = true, url });

        } //AtualizarEndereco

        #endregion

        #region Private methods aux

        private async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid id) {

            var fornecedor = await _fornecedorRepository.ObterFornecedorEndereco(id);
            var fornecedorVM = _mapper.Map<FornecedorViewModel>(fornecedor);

            return fornecedorVM;

        } //ObterFornecedorEndereco

        private async Task<FornecedorViewModel> ObterFornecedorProdutosEndereco(Guid id)
        {

            var fornecedor = await _fornecedorRepository.ObterFornecedorProdutosEndereco(id);
            var fornecedorVM = _mapper.Map<FornecedorViewModel>(fornecedor);

            return fornecedorVM;

        } //ObterFornecedorProdutosEndereco

        #endregion

    }
}
