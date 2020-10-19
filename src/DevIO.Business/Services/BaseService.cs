using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Business.Notificacoes;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevIO.Business.Services
{
    public abstract class BaseService
    {

        private readonly INotificadorErro _notificadorErro;

        protected BaseService(INotificadorErro notificadorErro)
        {
            _notificadorErro = notificadorErro ?? throw new ArgumentNullException(nameof(notificadorErro));
        }

        /// <summary>
        /// Metodo para notificar um erro 
        /// </summary>
        /// <param name="message">Mensagem de erro</param>
        protected void NotificarErro(string message) {
            _notificadorErro.Handle(new NotificacaoErro(message));
        } //NotificarErro

        /// <summary>
        /// Metodo para notificar um erro
        /// </summary>
        /// <param name="validationResult">ValidationResult eh uma colecao de erros obtido da validacao da entidade via FluentValidation</param>
        protected void NotificarErro(ValidationResult validationResult) {
            
            foreach (var erro in validationResult.Errors)
            {
                NotificarErro(erro.ErrorMessage);
            } //foreach

        } //NotificarErro

        /// <summary>
        /// Executa a validacao passando um tipo de validacao e uma entidade
        /// </summary>
        /// <typeparam name="TV">Tipo de Validacao</typeparam>
        /// <typeparam name="TE">Tipo de Entidade</typeparam>
        /// <param name="validacao"></param>
        /// <param name="entidade"></param>
        /// <returns></returns>
        protected bool ExecutarValidacao<TV, TE>(TV validacao, TE entidade) where TV: AbstractValidator<TE> where TE: Entity 
        {
            var validator = validacao.Validate(entidade);

            if(validator.IsValid)
                return true;

            NotificarErro(validator);

            return false;
        }

    } //class
} //namespave
