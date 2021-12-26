#nullable disable
using AutoMapper;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SERGETStore.App.ViewModels;
using SERGETStore.Business.Interfaces;

namespace SERGETStore.App.Controllers;

public class FornecedoresController : Controller
{
    private readonly IFornecedorRepository fornecedorRepository;
    private readonly IEnderecoRepository enderecoRepository;
    private readonly IMapper mapper;
    public FornecedoresController(IFornecedorRepository repository, IMapper mapper, IEnderecoRepository enderecoRepository)
    {
        this.fornecedorRepository = repository;
        this.mapper = mapper;
        this.enderecoRepository = enderecoRepository;
    }

    public async Task<IActionResult> Index()
    {
        var produtos = await fornecedorRepository.ObterTodos();
        var produtosVM = mapper.Map<IEnumerable<FornecedorViewModel>>(produtos);
        return View(produtosVM);
        //return View(await repository.ObterTodos());
    }


    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var fornecedorViewModel = await ObterFornecedorEndecreco(id.Value);


        if (fornecedorViewModel == null)
        {
            return NotFound();
        }

        return View(fornecedorViewModel);
    }


    public IActionResult Create()
    {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(FornecedorViewModel fornecedorViewModel)
    {
        if (!ModelState.IsValid)
            return View(fornecedorViewModel);

        var fornecedor = mapper.Map<Fornecedor>(fornecedorViewModel);
        await fornecedorRepository.Adiciontar(fornecedor);

        return RedirectToAction(nameof(Index));
    }


    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
            return NotFound();

        var fornecedorViewModel = await ObterFornecedorProdutosEndereco(id.Value);

        if (fornecedorViewModel == null)
        {
            return NotFound();
        }

        return View(fornecedorViewModel);
    }


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
            await fornecedorRepository.Atualizar(fornecedor);
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

    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
            return NotFound();


        var fornecedorViewModel = await ObterFornecedorEndecreco(id.Value);

        if (fornecedorViewModel == null)
            return NotFound();

        return View(fornecedorViewModel);
    }

    // POST: Fornecedores/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var fornecedorViewModel = await ObterFornecedorEndecreco(id);

        if (fornecedorViewModel is null)
            return NotFound();

        await fornecedorRepository.Remover(id);

        return RedirectToAction(nameof(Index));
    }

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
    public async Task<IActionResult> AtualizarEndereco(Guid id)
    {
        var fornecedor = await ObterFornecedorEndecreco(id);
        if (fornecedor is null)
            return NotFound();

        return PartialView("_AtualizarEndereco", new FornecedorViewModel { Endereco = fornecedor.Endereco });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AtualizarEndereco(FornecedorViewModel fornecedorViewModel)
    {
        //REMOVE ÍTEMS QUE NÃO FAZEM PARTE DESTE FLUXO, POIS AQUI ESTAMOS TRATANDO APEANS DO ENDEREÇO.
        //SERIA O CASO DE CRIAR UMA CONTROLLER SÓ PARA O ENDEREÇO.
        ModelState.Remove("Nome");
        ModelState.Remove("Documento");


        if(!ModelState.IsValid) 
            return PartialView("_AtualizarEndereco", fornecedorViewModel);

        var nvEndereco = mapper.Map<Endereco>(fornecedorViewModel.Endereco);
        await enderecoRepository.Atualizar(nvEndereco);

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
