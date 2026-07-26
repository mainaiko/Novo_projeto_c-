using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Middleware;
using Backend.Services;

//Incia o motor da aplicação
var builder = WebApplication.CreateBuilder(args);

//Configura os controllers com serialização JSON configurada para enums como string
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        //Serializa enums como string (ex: "Despesa" ao invés de 0)
        //Facilita a leitura no front-end e em ferramentas de debug.    
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

//AddOpenApi() e um metodo que adiciona o Swagger para documentação da API.
builder.Services.AddOpenApi();

//O DbContext com SQLite garante a persistência dos dados sem a necessidade de um servidor de banco de dados externo.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=gastos.db"));

//Registro dos serviços com Scoped lifetime (uma instância por request HTTP).
builder.Services.AddScoped<IPessoaService, PessoaService>();
builder.Services.AddScoped<ITransacaoService, TransacaoService>();

//CORS: permite que o front-end React (rodando em localhost:5173) faça requisições para esta API.
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

//Middleware de exceções deve ser o primeiro para capturar erros de todo o pipeline.
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//UseCors permite que o front-end faça requisições para esta API.
app.UseCors("FrontendPolicy");

//UseHttpsRedirection redireciona as requisições HTTP para HTTPS.
app.UseHttpsRedirection();

//MapControllers mapeia os endpoints da aplicação.
app.MapControllers();

// EnsureCreated() cria o banco e as tabelas automaticamente na primeira execução.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

//Inicia o servidor.
app.Run();
