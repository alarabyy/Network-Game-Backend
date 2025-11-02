using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Presentation.BackgroundWorkers;
using Quartz;
using System.Text;

namespace Presentation.Extensions;

public static class WebApiServiceExtensions
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        // --- Authentication / Authorization ---
        services.AddAuthenticationJwtBearer(s => s.SigningKey = configuration["JwtSettings:Key"]);
        
        services.AddAuthentication(o =>
        {
            o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        });
        services.AddAuthorization();

        // --- FastEndpoints ---
        services.AddFastEndpoints(o =>
        {
            // Scan the Presentation assembly explicitly
            o.Assemblies = new[] { typeof(WebApiServiceExtensions).Assembly };
        });

        // --- Swagger ---
        services.SwaggerDocument(o =>
        {
            o.DocumentSettings = s =>
            {
                s.Title = "Global Truth Watch API";
                s.Version = "v1";
            };
        });

        // --- CORS ---
        var allowedOrigins = configuration.GetSection("CorsSettings:AllowedOrigins").Get<string[]>();
        if (allowedOrigins == null || allowedOrigins.Length == 0)
            throw new InvalidOperationException("CORS AllowedOrigins is not configured in appsettings.json");

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.WithOrigins(allowedOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });
        
        services.AddHostedService<RssDiscoveryWorker>();
        services.AddQuartz(q =>
        {
            var jobKey = new JobKey("SendWeeklyEmailJob");
            q.AddJob<SendWeeklyEmailJob>(opts => opts.WithIdentity(jobKey));
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("WeeklyEmailTrigger")
                .WithCronSchedule("0 0 1 ? * SAT")
            ); 
        });

        return services;
    }
    
    public static void UsePresentationServices(this WebApplication app)
    {
        app.UseExceptionHandler("/error");
        app.Map("/error", () => Results.Problem("Unexpected error occurred"));

        app.UseCors();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseFastEndpoints(); // keep after auth
        app.UseSwaggerGen();
    }
}