using System;
using System.ComponentModel.DataAnnotations;

namespace PF.Business.Models
{
    public class Endereco
    {
        public Guid Id { get; set; }
        public Guid FornecedorId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(200)]
        public string Logradouro { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(50)]
        public string Numero { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(250)]
        public string Complemento { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(8)]
        public string Cep { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(100)]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(100)]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(50)]
        public string Estado { get; set; }

        /* EF Relation */
        public Fornecedor Fornecedor { get; set; }
    }
}