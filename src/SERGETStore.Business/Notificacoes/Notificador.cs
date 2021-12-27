using SERGETStore.Business.Interfaces;

namespace SERGETStore.Business.Notificacoes
{
    public class Notificador : INotificador
    {
        private readonly List<Notificacao> notificacoes;

        public Notificador()
        {
            this.notificacoes = new List<Notificacao>();
        }

        public void Handle(Notificacao notificacao)
        {
            notificacoes.Add(notificacao);
        }

        public List<Notificacao> ObterNotificacoes()
        {
            return notificacoes;
        }

        public bool TemNotificacao()
        {
            return notificacoes.Any();  
        }
    }
}
