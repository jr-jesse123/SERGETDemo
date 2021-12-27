#nullable disable
using Microsoft.AspNetCore.Mvc;
using SERGETStore.Business.Interfaces;

namespace SERGETStore.App.ViewModels
{
    public abstract class BaseController : Controller
    {
        public BaseController(INotificador notificador)
        {
            Notificador = notificador;
        }

        public INotificador Notificador { get; }

        protected bool OperacaoValida() => !Notificador.TemNotificacao();
    }
}
