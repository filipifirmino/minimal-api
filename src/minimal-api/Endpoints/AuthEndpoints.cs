using Microsoft.AspNetCore.Mvc;
using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.UseCases;

namespace minimal_api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Authentication");

        group.MapPost("/login", async ([FromBody] LoginRequestDto request, ILoginProcess loginProcess) =>
        {
            try
            {
                var result = await loginProcess.HandleAsync(request);
                
                if (result == null)
                    return Results.Unauthorized();

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        })
        .WithName("Login")
        .WithSummary("Autenticar usuário")
        .WithDescription("Realiza login no sistema e retorna um token de autenticação")
        .WithOpenApi()
        .AllowAnonymous()
        .Produces<LoginResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces<object>(StatusCodes.Status400BadRequest);
    }
}


