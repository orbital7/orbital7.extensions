//namespace Orbital7.Extensions.EntityFrameworkCore;

///// <summary>
///// Helper functions for configuring identity services.
///// </summary>
//public static class IdentityBuilderExtensions
//{
//    /// <summary>
//    /// Adds the default token providers used to generate tokens for reset passwords, change email
//    /// and change telephone number operations, and for two factor authentication token generation.
//    /// </summary>
//    /// <param name="builder">The current <see cref="IdentityBuilder"/> instance.</param>
//    /// <returns>The current <see cref="IdentityBuilder"/> instance.</returns>
//    public static IdentityBuilder AddDefaultTokenProviders(this IdentityBuilder builder)
//    {
//        var userType = builder.UserType;
//        //var dataProtectionProviderType = typeof(DataProtectorTokenProvider<>).MakeGenericType(userType);
//        var phoneNumberProviderType = typeof(PhoneNumberTokenProvider<>).MakeGenericType(userType);
//        var emailTokenProviderType = typeof(EmailTokenProvider<>).MakeGenericType(userType);
//        var authenticatorProviderType = typeof(AuthenticatorTokenProvider<>).MakeGenericType(userType);
//        return builder//.AddTokenProvider(TokenOptions.DefaultProvider, dataProtectionProviderType)
//            .AddTokenProvider(TokenOptions.DefaultEmailProvider, emailTokenProviderType)
//            .AddTokenProvider(TokenOptions.DefaultPhoneProvider, phoneNumberProviderType)
//            .AddTokenProvider(TokenOptions.DefaultAuthenticatorProvider, authenticatorProviderType);
//    }
//}