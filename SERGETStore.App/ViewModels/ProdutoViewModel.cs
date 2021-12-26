using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static SERGETStore.App.ViewModels.Msgs;
namespace SERGETStore.App.ViewModels
{
    public class ProdutoViewModel
    {
        
        [Key]
        public Guid Id { get; set; }


        [Required(ErrorMessage = Msgs.msgCampoObrigatorio)]
        [DisplayName("Fornecedor")]
        public Guid FornecedorId { get; set; }

        [Required(ErrorMessage = Msgs.msgCampoObrigatorio)]
        [StringLength(200,
            ErrorMessage =Msgs.msgErroTamanhoMaximo,MinimumLength =2)]
        public string Nome { get; set; }


        [DisplayName("Descrição")]
        [Required(ErrorMessage = msgCampoObrigatorio, AllowEmptyStrings =false)]
        [StringLength(1000,ErrorMessage = msgErroTamanhoMaximo)]
        public string Descricao { get; set; }

        [DisplayName("Imagem do Produto")]
        public IFormFile? ImagemUpload { get; set; } 
        
        public string? Imagem { get; set; } 
        [Required(ErrorMessage = msgCampoObrigatorio)]
        public decimal Valor { get; set; }
        [ScaffoldColumn(false)]
        public DateTime DataCadastro { get; set; }
        [DisplayName("Ativo?")]
        public bool Ativo { get; set; }
        
        public FornecedorViewModel? Fornecedor { get; set; }  
        /// <summary>
        /// Lista de fornecedores disponíveis para escolha durante edição
        /// </summary>
        public IEnumerable<FornecedorViewModel>? Fornecedores { get; set; } 
    }
}
