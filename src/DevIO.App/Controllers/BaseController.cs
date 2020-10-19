using DevIO.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DevIO.App.Controllers
{
    public abstract class BaseController : Controller 
    {

        private readonly INotificadorErro _notificadorErro;
        
        protected BaseController(INotificadorErro notificadorErro)
        {
            _notificadorErro = notificadorErro ?? throw new ArgumentNullException(nameof(notificadorErro));
        }

        protected bool OperacaoValida() => !_notificadorErro.TemNotificacaoErro();

    }
}
