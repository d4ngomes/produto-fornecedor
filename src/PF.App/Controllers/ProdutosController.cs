using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PF.Business.Interfaces;
using PF.Business.Models;
using PF.Data.Context;

namespace PF.App.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IFornecedorRepository _fornecedorRepository;

        public ProdutosController(IProdutoRepository produtoRepository, IFornecedorRepository fornecedorRepository)
        {
            _produtoRepository = produtoRepository;
            _fornecedorRepository = fornecedorRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _produtoRepository.ObterProdutosFornecedores());
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var produto = await _produtoRepository.ObterProdutoFornecedor(id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["Fornecedores"] = new SelectList(await _fornecedorRepository.ObterTodos(), "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Produto produto)
        {
            if (ModelState.IsValid)
            {
                await _produtoRepository.Adicionar(produto);
                await _produtoRepository.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Fornecedores"] = new SelectList(await _fornecedorRepository.ObterTodos(), "Id", "Nome", produto.FornecedorId);
            return View(produto);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var produto = await _produtoRepository.ObterPorId(id);
            if (produto == null)
            {
                return NotFound();
            }
            ViewData["FornecedorId"] = new SelectList(await _fornecedorRepository.ObterTodos(), "Id", "Nome", produto.FornecedorId);
            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Produto produto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _produtoRepository.Atualizar(produto);
                    await _produtoRepository.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["FornecedorId"] = new SelectList(await _fornecedorRepository.ObterTodos(), "Id", "Nome", produto.FornecedorId);
            return View(produto);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var produto = await _produtoRepository.ObterPorId(id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var produto = await _produtoRepository.ObterPorId(id);
            if (produto == null) return NotFound();
            await _produtoRepository.Remover(id);
            await _produtoRepository.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
