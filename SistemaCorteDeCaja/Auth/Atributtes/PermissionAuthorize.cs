using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using SistemaCorteDeCaja.Auth.Services;
using System.Reflection;
using System.Security.Claims;


namespace SistemaCorteDeCaja.Auth.Atributtes
{
    public class PermissionAuthorize : TypeFilterAttribute
    {
        public PermissionAuthorize()
            : base(typeof(PermissionValidationFilter))
        {

        }

        private class PermissionValidationFilter(AuthService authenticationService) : IAsyncAuthorizationFilter
        {
            private readonly AuthService _authService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));

            public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
            {
                var user = context.HttpContext.User;

                if (user == null || (!user.Identity?.IsAuthenticated ?? false))
                {
                    context.Result = new ForbidResult();
                    return;
                }

                if (context.ActionDescriptor is ControllerActionDescriptor actionDescriptor)
                {
                    MethodInfo? methodInfo = actionDescriptor.MethodInfo;
                    if (methodInfo == null) return;


                    PermissionPolicy? permissionPolicy = methodInfo.GetCustomAttribute<PermissionPolicy>();
                    if (permissionPolicy == null) return;


                    string? subClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    string? userRole = user.FindFirst(ClaimTypes.Role)?.Value;

                    if (!int.TryParse(subClaim, out var userId))
                    {
                        context.Result = new ForbidResult("User is not valid");
                        return;
                    }

                    if (string.IsNullOrEmpty(userRole))
                    {
                        context.Result = new ForbidResult("Role is not valid");
                        return;
                    }

                    bool hasPermission = await _authService.CheckUserPermissionAsync(
                        userId,
                        userRole,
                        permissionPolicy.Action,
                        permissionPolicy.Subject
                    );

                    if (!hasPermission)
                    {
                        context.Result = new ForbidResult();
                    }

                }
            }
        }

    }


}
