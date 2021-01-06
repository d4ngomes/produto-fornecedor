using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PF.Business.Models
{
    public class Produto
    {
        public Guid Id { get; set; }
        public Guid FornecedorId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(200)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(1000)]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(100)]
        public string Imagem { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Valor { get; set; }

        [Display(Name = "Data de Cadastro")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        /* EF Relations */
        public Fornecedor Fornecedor { get; set; }
    }
}