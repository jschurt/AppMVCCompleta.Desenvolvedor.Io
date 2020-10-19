using DevIO.App.Extensions.CustomAuthorizations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace DevIO.App.Extensions.TagHelpers
{

    /// <summary>
    /// Tag helper para desabilitar um link ou nao baseado nos claims do usuario. 
    /// Qualquer link que contiver os atributos html "disable-by-claim-name" e "disable-by-claim-value" serao "transformados" em taghelpers
    /// </summary>
    [HtmlTargetElement("a", Attributes = "disable-by-claim-name")]
    [HtmlTargetElement("a", Attributes = "disable-by-claim-value")]
    public class DesabilitaLinkByClaimTagHelper : TagHelper
    {

        private readonly IHttpContextAccessor _contextAccessor;

        public DesabilitaLinkByClaimTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        [HtmlAttributeName("disable-by-claim-name")]
        public string IdentityClaimName { get; set; }

        [HtmlAttributeName("disable-by-claim-value")]
        public string IdentityClaimValue { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (output == null)
                throw new ArgumentNullException(nameof(context));

            var temAcesso = CustomAuthorization.ValidarClaimsUsuario(_contextAccessor.HttpContext, IdentityClaimName, IdentityClaimValue);

            if (!temAcesso)
            {
                output.Attributes.RemoveAll("href");
                output.Attributes.Add(new TagHelperAttribute("style", "cursor: not-allowed"));
                output.Attributes.Add(new TagHelperAttribute("title", "Voce nao tem permissao"));
            }

        } //Process

    } //class

} //namespace
