using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DevIO.App.Extensions.CustomAuthorizations
{

    /// <summary>
    /// Atributo que utilizara o Filtro de Claims (RequisitoClaimFilter)
    /// </summary>
    public class ClaimsAuthorizeAttribute : TypeFilterAttribute
    {
        public ClaimsAuthorizeAttribute(string claimName, string claimValue) : base(typeof(RequisitoClaimFilter))
        {
            Arguments = new object[] { new Claim(claimName, claimValue) };
        }
    }
}
