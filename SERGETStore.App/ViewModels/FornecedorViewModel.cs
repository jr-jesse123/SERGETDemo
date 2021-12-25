using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SERGETStore.App.ViewModels;
#pragma warning disable CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere declará-lo como anulável.

public class FornecedorViewModel
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = Msgs.msgCampoObrigatorio)]
    [StringLength(100,ErrorMessage = Msgs.msgErroTamanhoMaximo)]
    public string Nome { get; set; }

    [Required(ErrorMessage = Msgs.msgCampoObrigatorio)]
    [StringLength(14, ErrorMessage = Msgs.msgErroTamanhoMaximo)]
    public string Documento { get; set; }

    [DisplayName("Tipo")]
    public int TipoFornecedor { get; set; }

    public EnderecoViewModel Endereco { get; set; }
    [DisplayName("Ativo?")]

    public bool Ativo { get; set; }

    public IEnumerable<ProdutoViewModel> Produtos { get; set; }
}

#pragma warning restore CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere declará-lo como anulável.