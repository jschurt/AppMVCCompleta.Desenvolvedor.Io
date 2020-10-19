using DevIO.App.Extensions.CustomAuthorizations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace DevIO.App.Extensions.TagHelpers
{

    /// <summary>
    /// Tag helper para renderizar um elemento ou nao baseado nos claims do usuario. 
    /// Qualquer elemento que contiver os atributos html "supress-by-claim-name" e "supress-by-claim-value" serao "transformados" em taghelpers
    /// </summary>
    //[HtmlTargetElement("a", Attributes ="supress-by-claim-name")]
    [HtmlTargetElement("*", Attributes = "supress-by-claim-name")]
    [HtmlTargetElement("*", Attributes = "supress-by-claim-value")]
    public class ApagaElementoByClaimTagHelper : TagHelper
    {

        private readonly IHttpContextAccessor _contextAccessor;

        public ApagaElementoByClaimTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        [HtmlAttributeName("supress-by-claim-name")]
        public string IdentityClaimName { get; set; }
        
        [HtmlAttributeName("supress-by-claim-value")] 
        public string IdentityClaimValue{ get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (output == null)
                throw new ArgumentNullException(nameof(context));

            var temAcesso = CustomAuthorization.ValidarClaimsUsuario(_contextAccessor.HttpContext, IdentityClaimName, IdentityClaimValue);

            if (!temAcesso)
                output.SuppressOutput();

        } //Process

    } //class

} //namespace
