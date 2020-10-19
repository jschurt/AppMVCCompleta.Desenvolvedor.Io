using DevIO.Business.Notificacoes;
using System.Collections.Generic;

namespace DevIO.Business.Interfaces
{
    public interface INotificadorErro
    {
        /// <summary>
        /// Retorna a existencia ou nao de notificacoes de erro
        /// </summary>
        /// <returns></returns>
        bool TemNotificacaoErro();

        /// <summary>
        /// Retorna lista de notificacoes de erro
        /// </summary>
        /// <returns></returns>
        List<NotificacaoErro> ObterNotificacoesErro();

        /// <summary>
        /// Manipula notificacao de erro quando ela for lancada
        /// </summary>
        /// <param name="notificacaoErro"></param>
        void Handle(NotificacaoErro notificacaoErro);

    }
}
