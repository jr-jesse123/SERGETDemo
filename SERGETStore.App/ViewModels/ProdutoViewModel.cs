using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static SERGETStore.App.ViewModels.Msgs;
namespace SERGETStore.App.ViewModels
{
    public class ProdutoViewModel
    {
        
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(200,
            ErrorMessage =Msgs.msgErroTamanhoMaximo,MinimumLength =2)]
        public string Nome { get; set; }
        [DisplayName("Descrição")]
        [Required(ErrorMessage = msgCampoObrigatorio, AllowEmptyStrings =false)]
        [StringLength(1000,ErrorMessage = msgErroTamanhoMaximo)]
        public string Descricao { get; set; }
        
        //public IFormFile ImagemUpload { get; set; }
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
