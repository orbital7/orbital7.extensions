using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Orbital7.Extensions.Apis;

public static class ApiExtensions
{
    private const string PROBLEM_DETAILS_TYPE = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
    private const string PROBLEM_JSON_CONTENT_TYPE = "application/problem+json";

    public static IServiceCollection UseJsonSerializationHelperOptionsInMappedEndpoints(
        this IServiceCollection serviceCollection)
    {
        return serviceCollection.ConfigureHttpJsonOptions(options =>
        {
            JsonSerializationHelper.SetSerializerOptions(
                options.SerializerOptions);
        });
    }

    public static IMvcBuilder UseJsonSerializationHelperOptionsInControllers(
        this IMvcBuilder builder)
    {
        return builder.AddJsonOptions(options =>
        {
            JsonSerializationHelper.SetSerializerOptions(
                options.JsonSerializerOptions);
        });
    }

    public static IMvcBuilder UseValidationProblemDetailsForInvalidModelStateInControllers(
        this IMvcBuilder builder)
    {
        return builder.ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Type = PROBLEM_DETAILS_TYPE,
                    Title = "One or more validation errors occurred.",
                    Status = StatusCodes.Status400BadRequest,
                    Instance = context.HttpContext.Request.Path,
                };

                return new BadRequestObjectResult(problemDetails)
                {
                    ContentTypes = { PROBLEM_JSON_CONTENT_TYPE }
                };
            };
        });
    }

    public static IApplicationBuilder UseProblemDetailsExceptionHandler(
        this WebApplication app)
    {
        return app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
                var exception = exceptionFeature?.Error;

                var detail = exception?.Message;
                var extensions = new Dictionary<string, object?>(StringComparer.Ordinal);

                // Add development-only details if in development mode.
                if (app.Environment.IsDevelopment())
                {
                    if (exception != null)
                    {
                        detail = exception.FlattenMessages();

                        extensions.Add(
                            "exception",
                            new Dictionary<string, object?>
                            {
                                { "Type", exception.GetType().FullName },
                                { "Source", exception.Source },
                                { "StackTrace", exception.StackTrace },
                            });
                    }

                    extensions.Add("headers", context.Request.Headers);
                }

                var problemDetails = new ProblemDetails()
                {
                    Type = PROBLEM_DETAILS_TYPE,
                    Title = "An unexpected error occurred.",
                    Detail = detail,
                    Status = StatusCodes.Status500InternalServerError,
                    Instance = context.Request.Path,
                    Extensions = extensions,
                };

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsJsonAsync(
                    problemDetails,
                    options: null,
                    contentType: PROBLEM_JSON_CONTENT_TYPE);
            });
        });
    }
}
