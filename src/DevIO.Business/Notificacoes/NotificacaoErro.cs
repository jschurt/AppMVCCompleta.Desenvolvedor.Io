using System;
using System.Collections.Generic;
using System.Text;

namespace DevIO.Business.Notificacoes
{

    public class NotificacaoErro
    {
        public NotificacaoErro(string mensagemErro)
        {
            MensagemErro = mensagemErro ?? throw new ArgumentNullException(nameof(mensagemErro));
        }

        public string MensagemErro { get; }
    
    } //class

} //namespace
