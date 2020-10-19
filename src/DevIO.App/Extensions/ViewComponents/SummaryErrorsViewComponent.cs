using DevIO.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.App.Extensions.ViewComponents
{
    public class SummaryErrorsViewComponent : ViewComponent
    {

        private readonly INotificadorErro _notificadorErro;

        public SummaryErrorsViewComponent(INotificadorErro notificadorErro)
        {
            _notificadorErro = notificadorErro ?? throw new ArgumentNullException(nameof(notificadorErro));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //Como o metodo ObterNotificacoesErro e o metodo InvokeAsync precisa ser Async, a chamada deve
            //ser feita via Task.FromResult()
            var notificacoesErro = await Task.FromResult(_notificadorErro.ObterNotificacoesErro());

            notificacoesErro.ForEach(ne => ViewData.ModelState.AddModelError(string.Empty, ne.MensagemErro));

            return View();

        } //InvokeAsync

    } //class
} //namespace
