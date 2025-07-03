using Microsoft.AspNetCore.Authentication.JwtBearer;
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
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(tokenGrantConfig.SigningKey))
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
}
