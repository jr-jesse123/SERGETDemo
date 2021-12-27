#nullable disable
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SERGETStore.App.ViewModels;
using SERGETStore.Business.Interfaces;
using SERGETStore.Business.Models;

namespace SERGETStore.App.Controllers;


public class FornecedoresController : Controller
{
    private readonly IFornecedorRepository fornecedorRepository;
    private readonly IFornecedorService fornecedorService;
    private readonly IMapper mapper;
    
    public FornecedoresController(IFornecedorRepository repository, 
                                  IMapper mapper, 
                                  IFornecedorService fornecedorService)
    {
        this.fornecedorRepository = repository;
        this.mapper = mapper;
        this.fornecedorService = fornecedorService;
    }
    [Route("lista-de-fornecedores")]
    public async Task<IActionResult> Index()
    {
        var produtos = await fornecedorRepository.ObterTodos();
        var produtosVM = mapper.Map<IEnumerable<FornecedorViewModel>>(produtos);
        return View(produtosVM);
    }

    [Route("daods-do-fornecedor/{id:guid}")]
    public async Task<IActionResult> Details(Guid id)
    {
        
        var fornecedorViewModel = await ObterFornecedorEndecreco(id);


        if (fornecedorViewModel == null)
        {
            return NotFound();
        }

        return View(fornecedorViewModel);
    }

    [Route("novo-fornecedor")]
    public IActionResult Create()
    {
        return View();
    }


    [Route("novo-fornecedor")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(FornecedorViewModel fornecedorViewModel)
    {
        if (!ModelState.IsValid)
            return View(fornecedorViewModel);

        var fornecedor = mapper.Map<Fornecedor>(fornecedorViewModel);
        await fornecedorService.Adicionar(fornecedor);
            

        return RedirectToAction(nameof(Index));
    }

    [Route("editar-fornecedor/{id:guid}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var fornecedorViewModel = await ObterFornecedorProdutosEndereco(id);

        if (fornecedorViewModel == null)
        {
            return NotFound();
        }

        return View(fornecedorViewModel);
    }

    [Route("editar-fornecedor/{id:guid}")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, FornecedorViewModel fornecedorViewModel)
    {
        if (id != fornecedorViewModel.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(fornecedorViewModel);

        var fornecedor = mapper.Map<Fornecedor>(fornecedorViewModel);

        try
        {
            await fornecedorService.Atualizar(fornecedor);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!FornecedorExists(fornecedorViewModel.Id))
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
    
    [Route("excluir-fornecedor/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        
        var fornecedorViewModel = await ObterFornecedorEndecreco(id);

        if (fornecedorViewModel == null)
            return NotFound();

        return View(fornecedorViewModel);
    }

    
    [Route("excluir-fornecedor/{id:guid}")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var fornecedorViewModel = await ObterFornecedorEndecreco(id);

        if (fornecedorViewModel is null)
            return NotFound();

        await fornecedorService.Remover(id);

        return RedirectToAction(nameof(Index));
    }

    [Route("obter-endereco-fornecedor/{id:guid}")]
    public async Task<IActionResult> ObterEndereco(Guid id)
    {
        var fornecedor = await ObterFornecedorEndecreco(id);
        if (fornecedor is null)
            return NotFound();

        return PartialView("_DetalhesEndereco",fornecedor);
    }

    /// <summary>
    /// Atualiza endereço de um fornecedor
    /// </summary>
    /// <param name="id">ID do fornecedor</param>
    /// <returns></returns>
    /// 
    [Route("atualizar-endereco-fornecedor/{id:guid}")]
    public async Task<IActionResult> AtualizarEndereco(Guid id)
    {
        var fornecedor = await ObterFornecedorEndecreco(id);
        if (fornecedor is null)
            return NotFound();

        return PartialView("_AtualizarEndereco", new FornecedorViewModel { Endereco = fornecedor.Endereco });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("atualizar-endereco-fornecedor/{id:guid}")]
    public async Task<IActionResult> AtualizarEndereco(FornecedorViewModel fornecedorViewModel)
    {
        //REMOVE ÍTEMS QUE NÃO FAZEM PARTE DESTE FLUXO, POIS AQUI ESTAMOS TRATANDO APEANS DO ENDEREÇO.
        //SERIA O CASO DE CRIAR UMA CONTROLLER SÓ PARA O ENDEREÇO.
        ModelState.Remove("Nome");
        ModelState.Remove("Documento");


        if(!ModelState.IsValid) 
            return PartialView("_AtualizarEndereco", fornecedorViewModel);

        var nvEndereco = mapper.Map<Endereco>(fornecedorViewModel.Endereco);
        await fornecedorService.AtualizarEndereco(nvEndereco);

        var url = 
            Url.Action(
                "ObterEndereco", "Fornecedores", 
                new { id = fornecedorViewModel.Endereco.FornecedorId });


        return Json(new {succees=true,url});

    }


    private bool FornecedorExists(Guid id)
    {
        return fornecedorRepository.ObterPorId(id) is not null;
    }

    private async Task<FornecedorViewModel> ObterFornecedorEndecreco(Guid id)
    {
        return mapper.Map<FornecedorViewModel>(await fornecedorRepository.ObterFornecedorEnderecoPorId(id));
    }
    private async Task<FornecedorViewModel> ObterFornecedorProdutosEndereco(Guid id)
    {
        return mapper.Map<FornecedorViewModel>(await fornecedorRepository.ObterFornecedorProdutosEnderecoPorId(id));
    }
}
