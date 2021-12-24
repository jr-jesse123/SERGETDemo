using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SERGETStore.App.ViewModels
{
    public class ProdutoViewModel
    {
        const string msgErroTamanhoMaximo = "O campo {0} precisa ter entre {2} e {1} caracteres";
        const string msgCampoObrigatorio = "O campo {0} é obrigatório";
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(200,
            ErrorMessage =,MinimumLength =2)]
        public string Nome { get; set; }
        [DisplayName("Descrição")]
        [Required(ErrorMessage = msgCampoObrigatorio, AllowEmptyStrings =false)]
        [StringLength(1000,ErrorMessage = msgErroTamanhoMaximo)]
        public string Descricao { get; set; }
        
        public IFormFile ImagemUpload { get; set; }
        public string Imagem { get; set; }
        [Required(ErrorMessage = msgCampoObrigatorio)]
        public decimal Valor { get; set; }
        [ScaffoldColumn(false)]
        public DateTime DataCadastro { get; set; }
        [DisplayName("Ativo?")]
        public bool Ativo { get; set; }
        public FornecedorViewModel Fornecedor { get; set; }
    }
}
