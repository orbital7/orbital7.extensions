using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Orbital7.Extensions.Jwt;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddTokenServiceJwtBearerAuthentication<TTokenInfo, TGetTokenInput>(
        this IServiceCollection services,
        TokenGrantConfig tokenGrantConfig)
        where TTokenInfo : TokenInfo
        where TGetTokenInput : GetTokenInput
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
                            .GetRequiredService<ITokenService<TTokenInfo, TGetTokenInput>>()
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

    public static IServiceCollection AddTokenServiceJwtBearerAuthentication<TTokenInfo, TGetTokenInput, TTokenService>(
        this IServiceCollection services,
        TokenGrantConfig tokenGrantConfig)
        where TTokenInfo : TokenInfo
        where TGetTokenInput : GetTokenInput
        where TTokenService : class, ITokenService<TTokenInfo, TGetTokenInput>
    {
        services.AddTokenServiceJwtBearerAuthentication<TTokenInfo, TGetTokenInput>(tokenGrantConfig);
        services.AddScoped<ITokenService<TTokenInfo, TGetTokenInput>, TTokenService>();
        return services;
    }

    public static void MapTokenServiceJwtBearerAuthenticationEndpoints<TTokenInfo, TGetTokenInput>(
        this WebApplication app,
        string getTokenRoute,
        string refreshTokenRoute,
        string revokeTokenRoute)
        where TTokenInfo : TokenInfo
        where TGetTokenInput : GetTokenInput
    {
        app.MapPost(
            getTokenRoute,
            async (
                [FromServices] ITokenService<TTokenInfo, TGetTokenInput> tokenService,
                [FromBody] TGetTokenInput input) =>
            {
                return await tokenService.GetTokenAsync(input);
            })
           .AllowAnonymous();

        app.MapPost(
            refreshTokenRoute,
            async (
                [FromServices] ITokenService<TTokenInfo, TGetTokenInput> tokenService,
                [FromBody] RefreshTokenInput input) =>
            {
                return await tokenService.RefreshTokenAsync(input);
            })
           .AllowAnonymous();

        app.MapPost(
            revokeTokenRoute,
            async (
                [FromServices] ITokenService<TTokenInfo, TGetTokenInput> tokenService,
                [FromBody] RevokeTokenInput input) =>
            {
                return await tokenService.RevokeTokenAsync(input);
            })
            .AllowAnonymous(); ;
    }
}
