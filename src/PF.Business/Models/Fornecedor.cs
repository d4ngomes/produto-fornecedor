using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PF.Business.Models
{
    public class Fornecedor
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres.", MinimumLength = 3)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(14, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres.", MinimumLength = 14)]
        public string Documento { get; set; }

        [Display(Name = "Tipo")]
        public TipoFornecedor TipoFornecedor { get; set; }

        public Endereco Endereco { get; set; }

        [Display(Name = "Ativo?")]
        public bool Ativo { get; set; }

        /* EF Relations */
        public IEnumerable<Produto> Produtos { get; set; }
    }
}