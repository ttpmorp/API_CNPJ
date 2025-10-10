using Microsoft.EntityFrameworkCore;
using CnpjApi.Data;
using CnpjApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao contêiner
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adicionar logging
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});

// Contexto do banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConsultaCnpjConnection")));

// Serviços
builder.Services.AddScoped<ICNPJService, CNPJService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configurar o pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Endpoint de teste para verificar a configuração
app.MapGet("/test-config", (IConfiguration configuration) =>
{
    var connectionString = configuration.GetConnectionString("ConsultaCnpjConnection");
    return new
    {
        ConnectionString = connectionString ?? "NÃO ENCONTRADO",
        HasConnectionString = !string.IsNullOrEmpty(connectionString)
    };
});

app.MapGet("/", () => "API CNPJ - Serviço de consulta de CNPJ");

app.Run();