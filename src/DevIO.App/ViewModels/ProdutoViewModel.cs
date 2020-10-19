using DevIO.App.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.App.ViewModels
{
    public class ProdutoViewModel
    {

        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} e obrigatorio")]
        [DisplayName("Fornecedor")]
        public Guid FornecedorId { get; set; }

        [Required(ErrorMessage = "O campo {0} e obrigatorio")]
        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2 )]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} e obrigatorio")]
        [StringLength(1000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Descricao { get; set; }

        public IFormFile ImagemUpload { get; set; }
        
        [DisplayName("Imagem do Produto")]
        public string Imagem { get; set; }

        [Moeda]
        [Required(ErrorMessage = "O campo {0} e obrigatorio")]
        public decimal Valor { get; set; }

        /// <summary>
        /// Na hora de fazer o scaffold (se eu fizer), esta coluna sera ignorada
        /// </summary>
        [ScaffoldColumn(false)]
        public DateTime DataCadastro { get; set; }

        [DisplayName("Ativo?")]
        public bool Ativo { get; set; }

        /*EF Relations */
        public FornecedorViewModel Fornecedor { get; set; }

        /// <summary>
        /// Lista que sera usada para popular combobox
        /// </summary>
        public IEnumerable<FornecedorViewModel> Fornecedores { get; set; }


    } //class

} //namespace
