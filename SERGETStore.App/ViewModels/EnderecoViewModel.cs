using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SERGETStore.App.ViewModels
{
    public class EnderecoViewModel
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required(ErrorMessage = Msgs.msgCampoObrigatorio)]
        [StringLength(200, ErrorMessage = Msgs.msgErroTamanhoMaximo, MinimumLength = 2)]
        public string Logradouro { get; set; }

        [Required(ErrorMessage = Msgs.msgCampoObrigatorio)]
        [StringLength(50, ErrorMessage = Msgs.msgErroTamanhoMaximo, MinimumLength = 1)]
        public string Numero { get; set; }

        public string Complemento { get; set; } = "";

        [Required(ErrorMessage = Msgs.msgCampoObrigatorio)]
        [StringLength(100, ErrorMessage = Msgs.msgErroTamanhoMaximo, MinimumLength = 2)]
        public string Bairro { get; set; }

        [Required(ErrorMessage = Msgs.msgCampoObrigatorio)]
        [StringLength(8, ErrorMessage = "O campo {0} precisa ter {1} caracteres", MinimumLength = 8)]
        public string Cep { get; set; }

        [Required(ErrorMessage = Msgs.msgCampoObrigatorio)]
        [StringLength(100, ErrorMessage = Msgs.msgErroTamanhoMaximo, MinimumLength = 2)]
        public string Cidade { get; set; }

        [Required(ErrorMessage = Msgs.msgCampoObrigatorio)]
        [StringLength(50, ErrorMessage = Msgs.msgErroTamanhoMaximo, MinimumLength = 2)]
        public string Estado { get; set; }

        [HiddenInput]
        public Guid FornecedorId { get; set; }
    }
}
