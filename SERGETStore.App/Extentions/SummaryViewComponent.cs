using Microsoft.AspNetCore.Mvc;
using SERGETStore.Business.Interfaces;

namespace SERGETStore.App.Extentions
{
    public class SummaryViewComponent : ViewComponent
    {
        public SummaryViewComponent(INotificador notificador)
        {
            Notificador = notificador;
        }

        public INotificador Notificador { get; }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var notificacoes = await Task.FromResult(Notificador.ObterNotificacoes());

            notificacoes.ForEach(n => ViewData.ModelState.AddModelError(String.Empty, n.Mensagem));


            return View();
        }
    }
}
