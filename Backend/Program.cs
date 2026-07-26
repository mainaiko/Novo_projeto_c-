using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Middleware;
using Backend.Services;

// Cria e configura o builder da aplicação web.
var builder = WebApplication.CreateBuilder(args);

// Registra os controllers e configura a serialização JSON.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Serializa enums como texto legível (ex: "Despesa" ao invés de 0).
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Adiciona o Swagger/OpenAPI para documentação interativa da API.
builder.Services.AddOpenApi();

// Configura o DbContext com SQLite como banco de dados local.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=gastos.db"));

// Registra os serviços com lifetime Scoped (uma instância por requisição HTTP).
builder.Services.AddScoped<IPessoaService, PessoaService>();
builder.Services.AddScoped<ITransacaoService, TransacaoService>();

// Configura CORS para permitir requisições do front-end React (localhost:5173).
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Middleware de exceções: primeiro do pipeline para capturar erros de todos os middlewares.
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Aplica a política de CORS configurada acima.
app.UseCors("FrontendPolicy");

// Redireciona requisições HTTP para HTTPS.
app.UseHttpsRedirection();

// Mapeia os endpoints dos controllers.
app.MapControllers();

// EnsureCreated() cria o banco e as tabelas automaticamente na primeira execução.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

// Inicia o servidor e aguarda requisições.
app.Run();
