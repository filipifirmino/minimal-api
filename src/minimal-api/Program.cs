using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using minimal_api.Dominio.Services;
using minimal_api.Dominio.UseCases;
using minimal_api.Endpoints;
using minimal_api.Infra.Context;
using minimal_api.Infra.Repository;

var builder = WebApplication.CreateBuilder(args);

// Configuração do DbContext
builder.Services.AddDbContext<DataContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

// Registro dos Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));

// Registro dos Services
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

// Registro dos UseCases
builder.Services.AddScoped<ICreateUserProcess, CreateUserProcess>();
builder.Services.AddScoped<ILoginProcess, LoginProcess>();

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Minimal API - Sistema de Gestão",
        Version = "v1",
        Description = "API REST para gerenciamento de usuários e veículos construída com .NET 9 Minimal APIs",
        Contact = new OpenApiContact
        {
            Name = "Suporte Técnico",
            Email = "suporte@minimalapi.com",
            Url = new Uri("https://github.com/seu-usuario/minimal-api")
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    // Configuração para autenticação Bearer/JWT
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = @"Autenticação JWT usando Bearer scheme.
                        
Entre com 'Bearer' [espaço] e então seu token no campo abaixo.

Exemplo: 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...'",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Ativa comentários XML (opcional - necessário criar o arquivo XML)
    // var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    // options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure o HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    // Habilita o Swagger
    app.UseSwagger();
    
    // Habilita o Swagger UI
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Minimal API v1");
        options.RoutePrefix = "swagger"; // Acesso via: /swagger
        options.DocumentTitle = "Minimal API - Documentação";
        
        // Configurações de UI
        options.DisplayRequestDuration();
        options.EnableDeepLinking();
        options.EnableFilter();
        options.ShowExtensions();
        options.EnableValidator();
        
        // Tema e customizações
        options.DefaultModelsExpandDepth(2);
        options.DefaultModelExpandDepth(2);
        options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
    });
}

app.UseHttpsRedirection();

// Mapear endpoints
app.MapVehicleEndpoints();
app.MapAuthEndpoints();
app.MapUserEndpoints();

app.Run();

