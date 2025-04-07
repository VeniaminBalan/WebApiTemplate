using Contracts;
using Entities;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace WepApiTemplate.Extensions;

public static class ServiceExtensions
{
    /// <summary>
    /// Cross-Origin Resource Sharing, give or restrict 
    /// access rights to applications from different domains. 
    /// </summary>
    /// <param name="services"></param>
    public static void ConfigureCors(this IServiceCollection services) 
        => services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder => 
                builder.AllowAnyOrigin() // WithOrigins("https://example.com") -> allow requests only from that concrete source.
                    .AllowAnyMethod() // WithMethods("POST", "GET") -> allow only specific HTTP methods.
                    .AllowAnyHeader()); // WithHeaders("accept", "context-type") -> allow only specific headers.
         });

    /// <summary>
    /// configure an IIS integration
    /// </summary>
    /// <param name="services"></param>
    public static void ConfigureISSIntegration(this IServiceCollection services) =>
        services.Configure<IISServerOptions>(options => { }); // see https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.iisserveroptions?view=aspnetcore-7.0

    public static void ConfigureLoggerService(this IServiceCollection services)
    {
        services.AddTransient<ILoggerManager, LoggerManager>();
    }
    
    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext<AppDbContext>(opts =>
            opts.UseMySQL(configuration.GetConnectionString("sqlConnection"), b => 
                b.MigrationsAssembly("WepApiTemplate")));

    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
        services.AddScoped<IRepositoryManager, RepositoryManager>();

    public static IMvcBuilder AddCustomCSVFormatter(this IMvcBuilder builder) =>
        builder.AddMvcOptions(config => 
            config.OutputFormatters.Add(new CsvOutputFormatter()));
}