using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookCatalog.Core.Api.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorizationFilterAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _value;
        private readonly string _key;
        public CustomAuthorizationFilterAttribute(string value, string key = "permission")
        {
            _value = value;
            _key = key;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();

                return;
            }

            var permissionClaim = context.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type.Equals(_key, StringComparison.OrdinalIgnoreCase)
                    && c.Value.Equals(_value, StringComparison.OrdinalIgnoreCase));

            if (permissionClaim is null)
            {
                context.Result = new ForbidResult();

                return;
            }
        }
    }
}
