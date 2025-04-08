using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;

namespace Orbital7.Extensions.AspNetCore;

public static class BuilderExtensions
{
    public static IApplicationBuilder UseNodeModulesStaticFiles(
        this IApplicationBuilder app)
    {
        var nodeModulesPath = Path.Combine(Directory.GetCurrentDirectory(), @"node_modules");
        if (Directory.Exists(nodeModulesPath))
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(nodeModulesPath),
                RequestPath = ""
            });
        }

        return app;
    }
}
