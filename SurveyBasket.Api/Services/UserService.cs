using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.Api.Abstractions;
using SurveyBasket.Api.Contracts.Users;
using SurveyBasket.Api.Entities;

namespace SurveyBasket.Api.Services;

public class UserService(UserManager<ApplicationUser> userManager) : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<Result<UserProfileResponse>> GetUserProfileAsync(string userId)
    {
        var user = await _userManager.Users
            .Where(c => c.Id == userId)
            //.Select(c => new UserProfileResponse(c.Email, c.UserName, c.FirstName,c.LastName))
            .ProjectToType<UserProfileResponse>()
            .SingleAsync();

        return Result.Success(user);
    }

    public async Task<Result> UpdateUserProfileAsync(string userId, UserProfileUpdateRequest request)
    {
        //var user = await _userManager.FindByIdAsync(userId);

        //user = request.Adapt(user);

        //await _userManager.UpdateAsync(user!);

        await _userManager.Users
            .Where(c => c.Id == userId)
            .ExecuteUpdateAsync(setters =>
                setters
                    .SetProperty(c => c.FirstName, request.FirstName)
                    .SetProperty(c => c.LastName, request.LastName)
            );

        return Result.Success();
    }

    public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId);

        var result = await _userManager.ChangePasswordAsync(user!, request.OldPassword, request.NewPassword);

        if (!result.Succeeded)
        {
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        return Result.Success();
    }
}
