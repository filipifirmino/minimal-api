using Microsoft.AspNetCore.Mvc;
using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.Mappers;
using minimal_api.Dominio.UseCases;
using minimal_api.Infra.Repository;

namespace minimal_api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/users").WithTags("Users");

        group.MapPost("/", async ([FromBody] CreateUserRequestDto request, ICreateUserProcess createUserProcess) =>
        {
            try
            {
                var result = await createUserProcess.HandleAsync(request);
                return Results.Created($"/api/users/{result.Id}", result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        })
        .WithName("CreateUser")
        .WithSummary("Criar novo usuário")
        .WithDescription("Cria um novo usuário no sistema com senha hasheada automaticamente")
        .WithOpenApi()
        .Produces<UserResponseDto>(StatusCodes.Status201Created)
        .Produces<object>(StatusCodes.Status400BadRequest);

        group.MapPut("/{id:guid}", async (Guid id, [FromBody] UpdateUserRequestDto request, IUserRepository userRepository) =>
        {
            try
            {
                if (id != request.Id)
                    return Results.BadRequest(new { error = "ID mismatch" });

                var existingUser = await userRepository.GetByIdAsync(id);
                if (existingUser == null)
                    return Results.NotFound(new { error = "User not found" });

                var updatedEntity = request.ToEntity(existingUser.Password);
                await userRepository.UpdateAsync(updatedEntity);

                return Results.Ok(updatedEntity.ToResponseDto());
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        })
        .WithName("UpdateUser")
        .WithSummary("Atualizar usuário")
        .WithDescription("Atualiza os dados de um usuário existente (exceto senha)")
        .WithOpenApi()
        .Produces<UserResponseDto>(StatusCodes.Status200OK)
        .Produces<object>(StatusCodes.Status400BadRequest)
        .Produces<object>(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:guid}", async (Guid id, IUserRepository userRepository) =>
        {
            try
            {
                var user = await userRepository.GetByIdAsync(id);
                if (user == null)
                    return Results.NotFound(new { error = "User not found" });

                await userRepository.DeleteAsync(user);
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        })
        .WithName("DeleteUser")
        .WithSummary("Deletar usuário")
        .WithDescription("Remove permanentemente um usuário do sistema")
        .WithOpenApi()
        .Produces(StatusCodes.Status204NoContent)
        .Produces<object>(StatusCodes.Status400BadRequest)
        .Produces<object>(StatusCodes.Status404NotFound);

        group.MapGet("/{id:guid}", async (Guid id, IUserRepository userRepository) =>
        {
            try
            {
                var user = await userRepository.GetByIdAsync(id);
                if (user == null)
                    return Results.NotFound(new { error = "User not found" });

                return Results.Ok(user.ToResponseDto());
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        })
        .WithName("GetUser")
        .WithSummary("Buscar usuário por ID")
        .WithDescription("Retorna os dados de um usuário específico pelo seu ID")
        .WithOpenApi()
        .Produces<UserResponseDto>(StatusCodes.Status200OK)
        .Produces<object>(StatusCodes.Status400BadRequest)
        .Produces<object>(StatusCodes.Status404NotFound);

        group.MapGet("/", async (IUserRepository userRepository) =>
        {
            try
            {
                var users = await userRepository.GetAllAsync();
                var usersDto = users.Select(u => u.ToResponseDto());
                return Results.Ok(usersDto);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        })
        .WithName("GetAllUsers")
        .WithSummary("Listar todos os usuários")
        .WithDescription("Retorna uma lista com todos os usuários cadastrados no sistema")
        .WithOpenApi()
        .Produces<IEnumerable<UserResponseDto>>(StatusCodes.Status200OK)
        .Produces<object>(StatusCodes.Status400BadRequest);
    }
}


