using DevIO.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevIO.Business.Notificacoes
{
    public class NotificadorErro : INotificadorErro
    {

        /// <summary>
        /// Lista global que contera todas as notificacoes durante todo o request. 
        /// </summary>
        private List<NotificacaoErro> _notificacaoErros = new List<NotificacaoErro>();

        /// <summary>
        /// Retorna a existencia ou nao de notificacoes de erro
        /// </summary>
        /// <returns></returns>
        public bool TemNotificacaoErro()
        {
            return _notificacaoErros.Any();
        } //TemNotificacao

        /// <summary>
        /// Retorna lista de notificacoes de erro
        /// </summary>
        /// <returns></returns>
        public List<NotificacaoErro> ObterNotificacoesErro()
        {
            return _notificacaoErros;
        } //ObterNotificacaoesErro

        /// <summary>
        /// Manipula notificacao de erro quando ela for lancada
        /// </summary>
        /// <param name="notificacaoErro"></param>
        public void Handle(NotificacaoErro notificacaoErro)
        {
            _notificacaoErros.Add(notificacaoErro);
        } //Handle

    } //class

} //namespace
