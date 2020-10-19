using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.App.Extensions
{
    public class MoedaAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            try
            {
                var moeda = Convert.ToDecimal(value, new CultureInfo("pt-BR"));
            }
            catch (Exception)
            {
                return new ValidationResult("Moeda em formato invalido");
            }

            return ValidationResult.Success;

        } //IsValid

    } //class

    /// <summary>
    /// O Adapter eh utilizado para criar a validacao no lado do cliente.
    /// Para usar um adapter, eu precico criar um provider (ver 
    /// </summary>
    public class MoedaAttributeAdapter : AttributeAdapterBase<MoedaAttribute>
    {

        public MoedaAttributeAdapter(MoedaAttribute attribute, IStringLocalizer stringLocalizer) : base(attribute, stringLocalizer)
        {

        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            //Adicionando atributo data-val, indicando que o elemento possui validacao (true)
            MergeAttribute(context.Attributes, "data-val", "true");

            //Adicionando atributo data-val-moeda, adicionando a mensagem de erro
            MergeAttribute(context.Attributes, "data-val-moeda", GetErrorMessage(context));

            //Por ser um campo moeda, o campo eh decimal, e automaticamente o atributo data-val-number seria criado.
            //Para colocar a minha mensagem (ao inves da mensagem padrao do framework), eu estou adicionando manualmente o
            //atributo data-val-number
            MergeAttribute(context.Attributes, "data-val-number", GetErrorMessage(context));

        } //AddValidation

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            return "Moeda em formato invalido";
        }

    } //class


    /// <summary>
    /// Provider para permitir a adicao da validacao de moeda no lado client.
    /// PARA FUNCIONAR, ESTE ADAPTER DEVE SER ADICIONADO VIA INJECAO DE DEPENDENCIA (startup)
    /// </summary>
    public class MoedaValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
    {

        private readonly IValidationAttributeAdapterProvider _baseProvider = new ValidationAttributeAdapterProvider();

        public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
        {

            if (attribute is MoedaAttribute moedaAttribute)
            {
                return new MoedaAttributeAdapter(moedaAttribute, stringLocalizer);
            }

            return _baseProvider.GetAttributeAdapter(attribute, stringLocalizer);

        }

    } //class

} //namespace
