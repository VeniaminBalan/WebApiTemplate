using System.Configuration;
using Contracts;
using Entities.DataTransferObjects;
using LoggerService;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Repository.DataShaping;
using WepApiTemplate.ActionFilters;
using WepApiTemplate.Extensions;

LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;

services.ConfigureCors();
services.ConfigureISSIntegration();
services.ConfigureLoggerService();
services.ConfigureRepositoryManager();
services.AddAutoMapper(typeof(Program));
services.AddScoped<ValidationFilterAttribute>();
services.AddScoped<ValidateCompanyExistsAttribute>();
services.AddScoped<ValidateEmployeeForCompanyExistsAttribute>();
services.AddScoped<IDataShaper<EmployeeDto>, DataShaper<EmployeeDto>>();

services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;
    config.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters()
    .AddCustomCSVFormatter();

services.ConfigureSqlContext(builder.Configuration);
services.Configure<ApiBehaviorOptions>(options => 
{ 
    options.SuppressModelStateInvalidFilter = true;
}); 

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    // will add middleware for using HSTS, which adds the Strict-Transport-Security header. 
    app.UseHsts();
}

app.ConfigureExceptionHandler(app.Services.GetService<ILoggerManager>() );
app.UseHttpsRedirection();

// enables using static files for the request. By default it will use a wwwroot folder
app.UseStaticFiles();
app.UseCors("CorsPolicy"); // from ServiceExtensions.ConfigureCors() method

// will forward proxy headers to the current request.
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseAuthorization();

app.MapControllers();

app.Run();