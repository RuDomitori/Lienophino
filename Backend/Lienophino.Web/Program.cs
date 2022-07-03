using Lienophino.ApiModel;
using Lienophino.Core.Queries;
using Lienophino.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
});
builder.Host.UseSerilog((context, provider, configuration) =>
{
    configuration
        .ReadFrom.Services(provider)
        .ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default");
    optionsBuilder.UseNpgsql(connectionString);
});
builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<AppDbContext>());

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddMediatR(typeof(GetMeals));


var app = builder.Build();

app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (context, httpContext) =>
    {
        context.Set("RemoteIp", httpContext.Connection.RemoteIpAddress);
    };
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configuring CORS to be able to run frontend by command "npm start"
app.UseCors(corsPolicyBuilder =>
{
    corsPolicyBuilder
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .WithOrigins("http://localhost:3000");
});

app.UseAuthorization();

app.MapControllers();

app.Run();