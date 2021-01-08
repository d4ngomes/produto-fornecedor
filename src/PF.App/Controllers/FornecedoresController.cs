using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PF.Business.Interfaces;
using PF.Business.Models;

namespace PF.App.Controllers
{
    public class FornecedoresController : Controller
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IEnderecoRepository _enderecoRepository;

        public FornecedoresController(IFornecedorRepository fornecedorRepository, IEnderecoRepository enderecoRepository)
        {
            _fornecedorRepository = fornecedorRepository;
            _enderecoRepository = enderecoRepository;
        }

        [Route("lista-de-fornecedores")]
        public async Task<IActionResult> Index()
        {
            return View(await _fornecedorRepository.ObterTodos());
        }

        [Route("dados-do-fornecedor/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var fornecedor = await _fornecedorRepository.ObterFornecedorEndereco(id);
            if (fornecedor == null)
            {
                return NotFound();
            }

            return View(fornecedor);
        }

        [Route("novo-fornecedor")]
        public IActionResult Create()
        {
            
            return View();
        }

        [Route("novo-fornecedor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Fornecedor fornecedor)
        {
            if (ModelState.IsValid)
            {
                await _fornecedorRepository.Adicionar(fornecedor);
                return RedirectToAction(nameof(Index));
            }
            return View(fornecedor);
        }

        [Route("editar-fornecedor/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var fornecedor = await _fornecedorRepository.ObterFornecedorProdutosEndereco(id);
            if (fornecedor == null)
            {
                return NotFound();
            }
            return View(fornecedor);
        }

        [Route("editar-fornecedor/{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Fornecedor fornecedor)
        {
            if (id != fornecedor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _fornecedorRepository.Atualizar(fornecedor);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(fornecedor);
        }

        [Route("excluir-fornecedor/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var fornecedor = await _fornecedorRepository.ObterPorId(id);
            if (fornecedor == null)
            {
                return NotFound();
            }

            return View(fornecedor);
        }

        [Route("excluir-fornecedor/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var fornecedor = await _fornecedorRepository.ObterPorId(id);
            if (fornecedor == null) return NotFound();
            await _fornecedorRepository.Remover(id);
            return RedirectToAction(nameof(Index));
        }

        [Route("atualizar-endereco-fornecedor/{id:guid}")]
        public async Task<IActionResult> AtualizaEndereco(Guid id)
        {
            var fornecedor = await _fornecedorRepository.ObterFornecedorEndereco(id);
            if (fornecedor == null)
            {
                return NotFound();
            }
            return View(fornecedor);
        }

        [Route("atualizar-endereco-fornecedor/{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AtualizaEndereco(Guid id, Fornecedor fornecedor)
        {
            if (id != fornecedor.Id)
            {
                return NotFound();
            }
            var endereco = await _enderecoRepository.ObterPorId(fornecedor.Endereco.Id);
            endereco.Cep = fornecedor.Endereco.Cep;
            endereco.Logradouro = fornecedor.Endereco.Logradouro;
            endereco.Numero = fornecedor.Endereco.Numero;
            endereco.Complemento = fornecedor.Endereco.Complemento;
            endereco.Bairro = fornecedor.Endereco.Bairro;
            endereco.Cidade = fornecedor.Endereco.Cidade;
            endereco.Estado = fornecedor.Endereco.Estado;
            await _enderecoRepository.Atualizar(endereco);
            return RedirectToAction(nameof(Details), new { id = endereco.FornecedorId });
        }
    }
}
