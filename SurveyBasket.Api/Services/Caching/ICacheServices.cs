﻿namespace SurveyBasket.Api.Services.Caching;

public interface ICacheServices
{
    Task<T?> GetAsync<T>(string Key, CancellationToken cancellationToken = default) where T : class;
    Task SetAsync<T>(string Key, T value, CancellationToken cancellationToken = default) where T : class;
    Task RemoveAsync(string Key, CancellationToken cancellationToken = default);
}
