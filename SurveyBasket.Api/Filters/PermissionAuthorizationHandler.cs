using Microsoft.AspNetCore.Authorization;

namespace SurveyBasket.Api.Filters;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirment>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirment requirement)
    {
        var user = context.User.Identity;

        var hasPermission = context.User.Claims.Any(c => c.Value == requirement.Permission);

        if (user is null || !user.IsAuthenticated || !hasPermission)
            return;

        context.Succeed(requirement);
        return;
    }
}
