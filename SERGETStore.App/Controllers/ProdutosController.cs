#nullable disable
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SERGETStore.App.Extentions;
using SERGETStore.App.ViewModels;
using SERGETStore.Business.Interfaces;
using SERGETStore.Business.Models;

namespace SERGETStore.App.Controllers
{
    //[Authorize]
    public class ProdutosController : BaseController
    {
        private readonly IProdutoRepository produtoRepository;
        private readonly IFornecedorRepository fornecedorRepository;
        private readonly IProdutoService produtoService;
        private readonly IMapper mapper;

        public ProdutosController(
                IProdutoRepository produtoRepository,
                IFornecedorRepository fornecedorRepository,
                IProdutoService produtoService,
                IMapper mapper,
                INotificador notificador) : base(notificador)
        {
            this.produtoRepository = produtoRepository;
            this.fornecedorRepository = fornecedorRepository;
            this.produtoService = produtoService;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        [Route("lista-de-produtos")]
        public async Task<IActionResult> Index()
        {
            var produtos = await produtoRepository.ObterProdutosFornecedores();
            var produtosViewModel = mapper.Map<IEnumerable<ProdutoViewModel>>(produtos);
            
            return View(produtosViewModel);
        }

        [AllowAnonymous]
        [Route("dados-do-produto/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var produtoViewModel = await ObterProdutoViewModelComFornecedores(id);
                
            if (produtoViewModel == null)
                return NotFound();
            
            return View(produtoViewModel);
        }

        //[ClaimsAuthorize("Produto","Adicionar")]
        [Route("novo-produto")]
        public async Task<IActionResult> Create()
        {
            var fornecedores = await ObterFornecedores();
            var produtoViewModel = new ProdutoViewModel() { Fornecedores = fornecedores };
            return View(produtoViewModel);
        }

        //[ClaimsAuthorize("Produto", "Adicionar")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("novo-produto")]
        public async Task<IActionResult> Create(ProdutoViewModel produtoViewModel)
        {

            produtoViewModel.Fornecedores = await ObterFornecedores();
            
            var fileName = GetFileName(produtoViewModel.ImagemUpload);
            if (!await UploadArquivo(produtoViewModel.ImagemUpload, fileName))
            {
                ModelState.AddModelError(nameof(produtoViewModel.ImagemUpload), "Não foi possível Fazer o Upload da Imagem");
                return View(produtoViewModel);
            }
            else
            {
                produtoViewModel.Imagem = fileName;
                
            }

            if (!ModelState.IsValid)
                return View(produtoViewModel);

            var produto = mapper.Map<Produto>(produtoViewModel);

            
            await produtoService.Adicionar(produto);

            if (!OperacaoValida())
                return View(produtoViewModel);
            

            return RedirectToAction(nameof(Index));
        }

        //[ClaimsAuthorize("Produto", "Editar")]
        [Route("editar-produto/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {

            var produtoViewModel = await ObterProdutoViewModelComFornecedores(id);

            if (produtoViewModel == null)
                return NotFound();
            
            return View(produtoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("editar-produto/{id:guid}")]
        //[ClaimsAuthorize("Produto", "Editar")]
        public async Task<IActionResult> Edit(Guid id, ProdutoViewModel produtoViewModel)
        {
            if (id != produtoViewModel.Id)
                return NotFound();

            var produtoAtualizacao = await ObterProdutoViewModelComFornecedores(id);

            produtoViewModel.Fornecedor = produtoAtualizacao.Fornecedor;

            produtoViewModel.Imagem = produtoAtualizacao.Imagem;

            if (!ModelState.IsValid)
                return View(produtoViewModel);

            if (produtoViewModel.ImagemUpload is not null)
            {
                var fileName = GetFileName(produtoViewModel.ImagemUpload);
                if (!await UploadArquivo(produtoViewModel.ImagemUpload, fileName))
                {
                    ModelState.AddModelError(nameof(produtoViewModel.ImagemUpload), "Não foi possível Fazer o Upload da Imagem");
                    return View(produtoViewModel);
                }
            }

            //TODO: MELHORAR DECLARATIVIDADE DESTE ESCOPO
            produtoAtualizacao.Nome = produtoViewModel.Nome;
            produtoAtualizacao.Descricao = produtoViewModel.Descricao;
            produtoAtualizacao.Valor = produtoViewModel.Valor;
            produtoAtualizacao.Ativo = produtoViewModel.Ativo;


            var produto = mapper.Map<Produto>(produtoAtualizacao);
            try
            {
                await produtoService.Atualizar(produto);
                if (!OperacaoValida())
                    return View(produtoViewModel);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProdutoExists(produtoAtualizacao.Id))
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

        [Route("excluir-produto/{id:guid}")]
        //[ClaimsAuthorize("Produto", "Excluir")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var produtoVM = await ObterProdutoViewModelComFornecedores(id);

            if (produtoVM is null)
                 return NotFound();
           
            //TODO: DELETAR
            return View(produtoVM);
        }

        
        //TODO: ALTERAR DELETES PARA HTTPDELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("excluir-produto/{id:guid}")]
        //[ClaimsAuthorize("Produto", "Excluir")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {   
            var produtoVM = await ObterProdutoViewModelComFornecedores(id);

            if (produtoVM is null)
                return NotFound();

            await produtoService.Remover(id);
            
            if (!OperacaoValida())
                return View(produtoVM);


            TempData["Sucesso"] = "Produto excluído com sucesso!";


            return RedirectToAction(nameof(Index));
        }

       
        //TODO: PENSAR EM ENVIAR ESTES MÉTODOS PARA HELPER PÚBLICO PARA SEREM TESTADOS.
        private async Task<ProdutoViewModel> ObterProdutoViewModelComFornecedores(Guid id)
        {
            //TODO: VERFICAR CHAMADA DUPLA PARA O BANCO
            var fornecedores = await fornecedorRepository.ObterTodos();

            var prodtuoFornecedor = await produtoRepository.ObterProdutoE_Fornecedor(id);

            var produtoViewModel = mapper.Map<ProdutoViewModel>(prodtuoFornecedor);

            produtoViewModel.Fornecedores = mapper.Map<IEnumerable<FornecedorViewModel>>(fornecedores);
            return produtoViewModel;
        }

        private async Task<IEnumerable<FornecedorViewModel>> ObterFornecedores()
        {
            var fornecedores = await fornecedorRepository.ObterTodos();
            var fornecedoresVM = mapper.Map<IEnumerable<FornecedorViewModel>>(fornecedores);
            return fornecedoresVM;
        }

        private async Task<bool> ProdutoExists(Guid id)
        {
            return await produtoRepository.ObterPorId(id) is not null;
        }


        private string GetFileName(IFormFile arquivo)
        {
            return Guid.NewGuid() + "_" + arquivo.FileName;
        }




        /// <summary>
        /// Tenta Realizar Upload do arquivo e retorna true em caso de sucesso
        /// </summary>
        /// <param name="arquivo"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private async Task<bool> UploadArquivo(IFormFile arquivo, string fileName)
        {
            //TODO: SEPARAR VALIDAÇÃO DO ARQUIVAMENTO EM FUNÇÕES DISTINTAS

            //TODO: ADICIONAR VALIDAÇÃO DE TAMANHO MÁXIMO NESTE ARQUIVO
            if (arquivo.Length <= 0) return false;
            
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Imagens", fileName);
            using (var stream = new FileStream(path, FileMode.Create))
            await arquivo.CopyToAsync(stream);

            return true;
        }

    }
}

