using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Orbital7.Extensions.Jwt;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddTokenServiceJwtBearerAuthentication(
        this IServiceCollection services,
        TokenGrantConfig tokenGrantConfig)
    {
        tokenGrantConfig.AssertIsComplete();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Events ??= new JwtBearerEvents();
                var onTokenValidated = options.Events.OnTokenValidated;

                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidAudience = tokenGrantConfig.ValidAudience,
                    ValidIssuer = tokenGrantConfig.ValidIssuer,
                    IssuerSigningKey = TokenServiceHelper.GetSigningKey(tokenGrantConfig.SigningKey),
                };
                options.Events.OnTokenValidated = async context =>
                {
                    await onTokenValidated(context);

                    var isTokenGrantValid = context.Principal != null ?
                        await context.HttpContext.RequestServices
                            .GetRequiredService<ITokenService>()
                            .IsTokenGrantValidAsync(context.Principal) :
                        false;

                    if (!isTokenGrantValid)
                    {
                        context.Fail(new RefreshTokenExpiredException());
                    }
                };
            });

        return services;
    }

    public static IServiceCollection AddTokenServiceJwtBearerAuthentication<TTokenService>(
        this IServiceCollection services,
        TokenGrantConfig tokenGrantConfig)
        where TTokenService : class, ITokenService
    {
        services.AddTokenServiceJwtBearerAuthentication(tokenGrantConfig);
        services.AddScoped<ITokenService, TTokenService>();
        return services;
    }

    public static void MapTokenServiceJwtBearerAuthenticationEndpoints(
        this WebApplication app,
        string getTokenRoute,
        string refreshTokenRoute,
        string revokeTokenRoute)
    {
        app.MapPost(
            getTokenRoute,
            async (
                [FromServices] ITokenService tokenService,
                [FromBody] GetTokenInput input) =>
            {
                return await tokenService.GetTokenAsync(input);
            })
           .AllowAnonymous();

        app.MapPost(
            refreshTokenRoute,
            async (
                [FromServices] ITokenService tokenService,
                [FromBody] RefreshTokenInput input) =>
            {
                return await tokenService.RefreshTokenAsync(input);
            })
           .AllowAnonymous();

        app.MapPost(
            revokeTokenRoute,
            async (
                [FromServices] ITokenService tokenService,
                [FromBody] RevokeTokenInput input) =>
            {
                return await tokenService.RevokeTokenAsync(input);
            })
            .AllowAnonymous(); ;
    }
}
