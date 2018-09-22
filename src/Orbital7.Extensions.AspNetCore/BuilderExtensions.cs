using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Microsoft.AspNetCore.Builder
{
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
}
