using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

        [Route("lista-de-produtos")]
        public async Task<IActionResult> Index()
        {
            return View(await _produtoRepository.ObterProdutosFornecedores());
        }

        [Route("dados-do-produto/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var produto = await _produtoRepository.ObterProdutoFornecedor(id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        [Route("novo-produto")]
        public async Task<IActionResult> Create()
        {
            ViewData["Fornecedores"] = new SelectList(await _fornecedorRepository.ObterTodos(), "Id", "Nome");
            return View();
        }

        [Route("novo-produto")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Produto produto)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Fornecedores"] = new SelectList(await _fornecedorRepository.ObterTodos(), "Id", "Nome", produto.FornecedorId);
                return View(produto);
            }

            var imgPrefixo = Guid.NewGuid() + "_";
            var img = HttpContext.Request.Form.Files;
            await UplodadImagem(img, imgPrefixo, produto);

            await _produtoRepository.Adicionar(produto);
            await _produtoRepository.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [Route("editar-produto/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var produto = await _produtoRepository.ObterProdutoFornecedor(id);
            if (produto == null)
            {
                return NotFound();
            }
            ViewData["Fornecedores"] = new SelectList(await _fornecedorRepository.ObterTodos(), "Id", "Nome", produto.FornecedorId);
            return View(produto);
        }

        [Route("editar-produto/{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Produto produto)
        {
            if (id != produto.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewData["Fornecedores"] = new SelectList(await _fornecedorRepository.ObterTodos(), "Id", "Nome", produto.FornecedorId);
                return View(produto);
            }
            var produtoFromDb = await _produtoRepository.ObterPorId(id);

            var imgPrefixo = Guid.NewGuid() + "_";
            var img = HttpContext.Request.Form.Files;
            if (img.Count > 0)
            {
                await UplodadImagem(img, imgPrefixo, produto);
                produtoFromDb.Imagem = produto.Imagem;
            }

            produtoFromDb.Nome = produto.Nome;
            produtoFromDb.Descricao = produto.Descricao;
            produtoFromDb.Valor = produto.Valor;
            produtoFromDb.Ativo = produto.Ativo;

            await _produtoRepository.Atualizar(produtoFromDb);
            await _produtoRepository.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [Route("excluir-produto/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var produto = await _produtoRepository.ObterProdutoFornecedor(id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        [Route("excluir-produto/{id:guid}")]
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

        private async Task UplodadImagem(IFormFileCollection img, string imgPrefixo, Produto produto)
        {
            if (img.Count <= 0)
            {
                var sourceImage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", "default-image.png");
                var destImage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imgPrefixo + ".png");
                System.IO.File.Copy(sourceImage, destImage);
                produto.Imagem = imgPrefixo + ".png";
            }
            else
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imgPrefixo + img[0].FileName);

                if (System.IO.File.Exists(path))
                {
                    ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome!");
                    return;
                }

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await img[0].CopyToAsync(stream);
                }

                produto.Imagem = imgPrefixo + img[0].FileName;
            }
        }
    }
}
