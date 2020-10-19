using DevIO.App.Extensions.CustomAuthorizations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using System;

namespace DevIO.App.Extensions.TagHelpers
{

    /// <summary>
    /// Tag helper para desabilitar um link ou nao baseado nos claims do usuario. 
    /// Qualquer link que contiver os atributos html "disable-by-claim-name" e "disable-by-claim-value" serao "transformados" em taghelpers
    /// </summary>
    [HtmlTargetElement("a", Attributes = "supress-by-action")]
    public class ApagaElementoByActionTagHelper : TagHelper
    {

        private readonly IHttpContextAccessor _contextAccessor;

        public ApagaElementoByActionTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        [HtmlAttributeName("supress-by-action")]
        public string ActionName { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (output == null)
                throw new ArgumentNullException(nameof(context));

            var action = _contextAccessor.HttpContext.GetRouteData().Values["action"].ToString();

            if (!ActionName.Contains(action))
            {
                output.SuppressOutput();
            }

        } //Process

    } //class

} //namespace
