using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using minimal_api.Dominio.Services;
using minimal_api.Dominio.UseCases;
using minimal_api.Dominio.Validators;
using minimal_api.Endpoints;
using minimal_api.Infra.Context;
using minimal_api.Infra.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.AddScoped<ICreateUserProcess, CreateUserProcess>();
builder.Services.AddScoped<ILoginProcess, LoginProcess>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();

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
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Minimal API v1");
        options.RoutePrefix = "swagger";
        options.DocumentTitle = "Minimal API - Documentação";
        
        options.DisplayRequestDuration();
        options.EnableDeepLinking();
        options.EnableFilter();
        options.ShowExtensions();
        options.EnableValidator();
        
        options.DefaultModelsExpandDepth(2);
        options.DefaultModelExpandDepth(2);
        options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
    });
}

app.UseHttpsRedirection();

app.MapVehicleEndpoints();
app.MapAuthEndpoints();
app.MapUserEndpoints();

app.Run();

