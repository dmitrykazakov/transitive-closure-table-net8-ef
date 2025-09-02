using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TransitiveClosureTable.Application.Services.Contracts;
using TransitiveClosureTable.Infrastructure.Data;
using TransitiveClosureTable.Infrastructure.Factories;
using TransitiveClosureTable.Infrastructure.Factories.Contracts;
using TransitiveClosureTable.Middleware;

var builder = WebApplication.CreateBuilder(args);
var useSqlServer = builder.Configuration.GetValue<bool>("UseSqlServer");

builder.Services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();

if (useSqlServer)
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")));
    builder.Services.AddScoped<IAppFactory, SqlServerAppFactory>();
}
else
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")));
    builder.Services.AddScoped<IAppFactory, PostgresAppFactory>();
}

builder.Services.AddScoped<INodeService>(serviceProvider =>
    serviceProvider.GetRequiredService<IAppFactory>().CreateServiceFactory().CreateNodeService());
builder.Services.AddScoped<ITreeService>(serviceProvider =>
    serviceProvider.GetRequiredService<IAppFactory>().CreateServiceFactory().CreateTreeService());
builder.Services.AddScoped<IExceptionJournalService>(serviceProvider =>
    serviceProvider.GetRequiredService<IAppFactory>().CreateServiceFactory().CreateExceptionJournalService());

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.SwaggerDoc("v0.0.1", new OpenApiInfo { Title = "Swagger", Version = "0.0.1" });
});

var app = builder.Build();

app.UseErrorHandlingMiddleware();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v0.0.1/swagger.json", "My API v0.0.1"); });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();