using System;
using System.IO;
using System.Reflection;
using System.Text;
using DataAccess.Data;
using DataAccess.UnitOfWorks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagementAPI.Options;

Console.OutputEncoding = Encoding.UTF8;

var builder = WebApplication.CreateBuilder(args: args);

var services = builder.Services;
var configuration = builder.Configuration;

// service configs.

// Controller config.
services.AddControllers(configure: option => option.SuppressAsyncSuffixInActionNames = false);

// DbContext config.
services.AddDbContextPool<ApplicationDbContext>(
    optionsAction: (provider, config) =>
    {
        var option = configuration
            .GetRequiredSection(key: "Database")
            .GetRequiredSection(key: "ApplicationDb")
            .Get<ApplicationDbContextOption>();

        config
            .UseSqlServer(
                connectionString: option.ConnectionString,
                sqlServerOptionsAction: databaseOptionsAction =>
                {
                    databaseOptionsAction
                        .CommandTimeout(commandTimeout: option.CommandTimeOut)
                        .EnableRetryOnFailure(maxRetryCount: option.EnableRetryOnFailure)
                        .MigrationsAssembly(
                            assemblyName: typeof(ApplicationDbContext).Assembly.GetName().Name
                        );
                }
            )
            .EnableSensitiveDataLogging(
                sensitiveDataLoggingEnabled: option.EnableSensitiveDataLogging
            )
            .EnableDetailedErrors(detailedErrorsEnabled: option.EnableDetailedErrors)
            .EnableThreadSafetyChecks(enableChecks: option.EnableThreadSafetyChecks)
            .EnableServiceProviderCaching(
                cacheServiceProvider: option.EnableServiceProviderCaching
            );
        ;
    }
);

// Swagger config.
services.AddSwaggerGen(setupAction: options =>
{
    options.SwaggerDoc(
        name: "v1",
        info: new()
        {
            Version = "v1",
            Title = "ProjectManagementAPI",
            Description = "An ASP.NET Core Web API for managing product.",
            License = new()
            {
                Name = "MIT",
                Url = new(uriString: "https://opensource.org/license/mit")
            }
        }
    );

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

    options.IncludeXmlComments(
        filePath: Path.Combine(path1: AppContext.BaseDirectory, path2: xmlFilename)
    );
});

// Other service configs.
services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection()
    .UseRouting()
    .UseSwagger()
    .UseSwaggerUI(setupAction: options =>
    {
        options.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "v1");
        options.RoutePrefix = string.Empty;
    });

app.MapControllers();

app.Run();
