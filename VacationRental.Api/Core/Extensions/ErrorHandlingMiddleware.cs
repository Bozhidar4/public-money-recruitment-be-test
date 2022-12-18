using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using VacationRental.Api.Models;

namespace VacationRental.Api.Core.Extensions
{
    public static class ErrorHandlingMiddleware
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        await context.Response.WriteAsync(new ErrorDetailsModel()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error?.Message
                        }.ToString());
                    }
                });
            });
        }
    }
}
