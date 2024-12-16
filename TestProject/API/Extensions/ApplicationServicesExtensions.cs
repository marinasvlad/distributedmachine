using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace API.Extensions;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddHttpClient<IDBApiService, DBApiService>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:5004");
        });

        services.AddHttpClient<IImageProcessorService, ImageProcessorService>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:5006");
        });
        services.Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = 1024 * 1024 * 100;
        });
        services.Configure<KestrelServerOptions>(options =>
        {
            options.Limits.MaxRequestBodySize = 1024 * 1024 * 100;
        });
        return services;
    }
}
