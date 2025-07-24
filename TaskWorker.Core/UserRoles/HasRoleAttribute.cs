using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OperationWorker.Core.UserRoles;
using System.Security.Claims;

namespace OperationWorker.Core.UserRoles
{
    public class HasRoleAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _requiredRole;

        public HasRoleAttribute(string requiredRole)
        {
            _requiredRole = requiredRole;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userRole = context.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            if (!RoleHierarchy.HasAccess(userRole, _requiredRole))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
