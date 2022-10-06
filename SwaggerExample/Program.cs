using SwaggerExample.Config.ApiVersion;
using SwaggerExample.Config.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

// Adicionando classes de configuração de versionamento e Swagger personalizado
builder.Services.ConfigureApiVersioning();
builder.Services.SwaggerGen();

var app = builder.Build();

// Adicionando classe de configuração sagger Ui personalizada 
app.SwaggerUi();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();