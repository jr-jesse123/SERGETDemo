#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SERGETStore.App.Data;
using SERGETStore.App.ViewModels;
using SERGETStore.Business.Interfaces;

namespace SERGETStore.App.Controllers
{

    public class ProdutosController : Controller
    {
        private readonly IProdutoRepository produtoRepository;
        private readonly IFornecedorRepository fornecedorRepository;
        private readonly IMapper mapper;

        public ProdutosController(
                IProdutoRepository produtoRepository,
                IFornecedorRepository fornecedorRepository,
                IMapper mapper)
        {
            this.produtoRepository = produtoRepository;
            this.fornecedorRepository = fornecedorRepository;
            this.mapper = mapper;
        }


        public async Task<IActionResult> Index()
        {
            var produtos = await produtoRepository.ObterProdutosFornecedores();
            var produtosViewModel = mapper.Map<IEnumerable<ProdutoViewModel>>(produtos);
            
            return View(produtosViewModel);
        }


        //TODO: TRATAR OS NULÁVEIS ATRAVÉS DE FILTERS
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
                return NotFound();

            var produtoViewModel = await ObterProdutoViewModelComFornecedores(id.Value);
                
            if (produtoViewModel == null)
                return NotFound();
            
            return View(produtoViewModel);
        }

        public async Task<IActionResult> Create()
        {
            var fornecedores = await ObterFornecedores();
            var produtoViewModel = new ProdutoViewModel() { Fornecedores = fornecedores };
            return View(produtoViewModel);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProdutoViewModel produtoViewModel)
        {

            //TODO: ADICIONAR UPLOAD DE IMAGEM

            produtoViewModel.Fornecedores = await ObterFornecedores();

            if (!ModelState.IsValid)
                return View(produtoViewModel);

            var produto = mapper.Map<Produto>(produtoViewModel);

            await produtoRepository.Adiciontar(produto);
            
            return View(produtoViewModel);
        }

        
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
                return NotFound();

            var produtoViewModel = await ObterProdutoViewModelComFornecedores(id.Value);

            if (produtoViewModel == null)
                return NotFound();
            
            return View(produtoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProdutoViewModel produtoViewModel)
        {
            if (id != produtoViewModel.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(produtoViewModel);

            var produto = mapper.Map<Produto>(produtoViewModel);
            try
            {
                await produtoRepository.Atualizar(produto);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProdutoExists(produtoViewModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
           
        }


        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
                return NotFound();

            var produtoVM = await ObterProdutoViewModelComFornecedores(id.Value);

            if (produtoVM is null)
                 return NotFound();
           
            //TODO: DELETAR
            return View(produtoVM);
        }

        
        //TODO: ALTERAR DELETES PARA HTTPDELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            
            var produtoVM = await ObterProdutoViewModelComFornecedores(id);

            if (produtoVM is null)
                return NotFound();

            await produtoRepository.Remover(id);

            return RedirectToAction(nameof(Index));
        }

       
        //TODO: PENSAR EM ENVIAR ESTES MÉTODOS PARA HELPER PÚBLICO PARA SEREM TESTADOS.
        private async Task<ProdutoViewModel> ObterProdutoViewModelComFornecedores(Guid id)
        {
            var fornecedores = await produtoRepository.ObterTodos();
            var prodtuoFornecedor = await produtoRepository.ObterProdutoE_Fornecedor(id);
            var produtoViewModel = mapper.Map<ProdutoViewModel>(prodtuoFornecedor);

            produtoViewModel.Fornecedores = mapper.Map<IEnumerable<FornecedorViewModel>>(fornecedores);
            return produtoViewModel;
        }

        //private async Task<ProdutoViewModel> PopularFornecedores(ProdutoViewModel produto)
        //{
        //    //var produto = mapper.Map<ProdutoViewModel>(await produtoRepository.ObterProdutoE_Fornecedor(id));
        //    var fornecedores = await produtoRepository.ObterTodos();
        //    produto.Fornecedores = mapper.Map<IEnumerable<FornecedorViewModel>>(fornecedores);
        //    return produto;
        //}

        private async Task<IEnumerable<FornecedorViewModel>> ObterFornecedores()
        {
            var fornecedores = await fornecedorRepository.ObterTodos();
            var fornecedoresVM = mapper.Map<IEnumerable<FornecedorViewModel>>(fornecedores);
            return fornecedoresVM;
        }

        //private async Task<ProdutoViewModel> ObterProdutoViewModel(Guid id)
        //{
        //    var produto = await produtoRepository.ObterPorId(id);
        //    var produtoVm = mapper.Map<ProdutoViewModel>(produto);
        //    return produtoVm;
        //}

        private async Task<bool> ProdutoExists(Guid id)
        {
            return await produtoRepository.ObterPorId(id) is not null;
        }


        

    }
}
