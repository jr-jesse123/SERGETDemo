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
    private readonly IFornecedorRepository repository;
    private readonly IMapper mapper;
    public FornecedoresController(IFornecedorRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var produtos = await repository.ObterTodos();
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
        await repository.Adiciontar(fornecedor);

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
            await repository.Atualizar(fornecedor);
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

        await repository.Remover(id);

        return RedirectToAction(nameof(Index));
    }

    private bool FornecedorExists(Guid id)
    {
        return repository.ObterPorId(id) is not null;
    }

    private async Task<FornecedorViewModel> ObterFornecedorEndecreco(Guid id)
    {
        return mapper.Map<FornecedorViewModel>(await repository.ObterFornecedorEnderecoPorId(id));
    }
    private async Task<FornecedorViewModel> ObterFornecedorProdutosEndereco(Guid id)
    {
        return mapper.Map<FornecedorViewModel>(await repository.ObterFornecedorEnderecoPorId(id));
    }
}
