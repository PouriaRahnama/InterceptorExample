
using InterceptorExample.web.Application.Services;
using InterceptorExample.web.Infrastructure.Common;
using InterceptorExample.web.Infrastructure.Persistence;
using InterceptorExample.web.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace InterceptorExample.web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<CustomInterceptor>();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<ShortenUrlService>();
            builder.Services.AddHttpContextAccessor();

            var settings = builder.Configuration.Get<AppSettings>();
            builder.Services.Configure<AppSettings>(builder.Configuration);

            builder.Services.AddDbContextPool<SqlServerApplicationDbContext>((sp, options) =>
            {
                if (settings is null) throw new Exception("Settings is null");
                options.UseSqlServer(settings.SqlServerDbConnection.ShortenLinkConnection).AddInterceptors(sp.GetRequiredService<CustomInterceptor>());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.MapPost("/shorten", async ([FromBody] ShortenRequest request, ShortenUrlService Services, CancellationToken cancellationToken) =>
            {
                return await Services.CreateShortenLink(request.Url, cancellationToken);
            });

            app.MapGet("/{short_Code}", async ([FromRoute(Name = "short_Code")] string shortenCode, ShortenUrlService Services, CancellationToken cancellationToken) =>
            {
                var destinationUrl = await Services.GetDestinationUrlAsync(shortenCode, cancellationToken);

                return Results.Ok(destinationUrl);
            });

            app.MapGet("/c/{short_Code}", async ([FromRoute(Name = "short_Code")] string shortenCode, ShortenUrlService Services, CancellationToken cancellationToken) =>
            {
                var destinationUrl = await Services.GetDestinationUrlAsync(shortenCode, cancellationToken);

                return Results.Redirect(destinationUrl);
            });
            app.Run();
        }
    }
}
