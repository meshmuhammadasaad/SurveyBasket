using Microsoft.AspNetCore.Authorization;

namespace SurveyBasket.Api.Filters;

public class PermissionRequirment(string permission) :IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}
