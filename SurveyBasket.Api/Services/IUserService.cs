using SurveyBasket.Api.Abstractions;
using SurveyBasket.Api.Contracts.Users;

namespace SurveyBasket.Api.Services;

public interface IUserService
{
    Task<Result<UserProfileResponse>> GetUserProfileAsync(string userId);
    Task<Result> UpdateUserProfileAsync(string userId, UserProfileUpdateRequest request);
    Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request);
}
