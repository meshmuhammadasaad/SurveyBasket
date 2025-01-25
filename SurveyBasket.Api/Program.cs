using Hangfire;
using Hangfire.Dashboard;
using HangfireBasicAuthenticationFilter;
using Scalar.AspNetCore;
using Serilog;
using SurveyBasket.Api.Extensions;
using SurveyBasket.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// My Services
builder.Services.AddDependencies(builder.Configuration);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseHangfireDashboard("/jobs", new DashboardOptions
{
    Authorization =
    [
        new HangfireCustomBasicAuthenticationFilter
        {
            User = app.Configuration.GetValue<string>("HangfireSettings:UserName"),
            Pass = app.Configuration.GetValue<string>("HangfireSettings:Password")
        }
    ],
    DashboardTitle = "Survey Basket Dashboard",
    //IsReadOnlyFunc = (DashboardContext context) => true
});

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
var notificationService = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<INotificationService>();

RecurringJob.AddOrUpdate("SendNewPollsNotificationAsync", () => notificationService.SendNewPollsNotificationAsync(null), Cron.Daily);

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler();

app.Run();
