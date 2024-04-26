using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace ECommerceAPI.API.Extensions
{
    public static class ConfigureExceptionHandlerException
    {
        public static void ConfigureExceptionHandler<T>(this IApplicationBuilder app , ILogger<T> logger)
        {
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = MediaTypeNames.Application.Json; //application/json

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if(contextFeature != null)
                    {
                        logger.LogError(contextFeature.Error.Message);

                        await context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                            Title = "Error"
                        }));
                    }
                });
            });
        }
    }
}
