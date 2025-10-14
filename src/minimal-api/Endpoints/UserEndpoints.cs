using FluentValidation;
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

        group.MapPost("/", async (
            [FromBody] CreateUserRequestDto request, 
            ICreateUserProcess createUserProcess,
            IValidator<CreateUserRequestDto> validator) =>
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
                return Results.BadRequest(new { errors });
            }

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

        group.MapPut("/{id:guid}", async (
            Guid id, 
            [FromBody] UpdateUserRequestDto request, 
            IUserRepository userRepository,
            IValidator<UpdateUserRequestDto> validator) =>
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
                return Results.BadRequest(new { errors });
            }

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

        group.MapGet("/", async (IUserRepository userRepository, int? pageNumber, int? pageSize) =>
        {
            try
            {
                if (!pageNumber.HasValue && !pageSize.HasValue)
                {
                    var users = await userRepository.GetAllAsync();
                    var usersDto = users.Select(u => u.ToResponseDto());
                    return Results.Ok(usersDto);
                }

                var pagination = new PaginationParameters
                {
                    PageNumber = pageNumber ?? 1,
                    PageSize = pageSize ?? 10
                };

                var pagedUsers = await userRepository.GetPagedAsync(pagination);
                var pagedResponse = new PagedResult<UserResponseDto>(
                    pagedUsers.Items.Select(u => u.ToResponseDto()),
                    pagedUsers.TotalCount,
                    pagedUsers.PageNumber,
                    pagedUsers.PageSize
                );

                return Results.Ok(pagedResponse);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        })
        .WithName("GetAllUsers")
        .WithSummary("Listar todos os usuários")
        .WithDescription("Retorna uma lista com todos os usuários cadastrados no sistema. Suporta paginação com parâmetros pageNumber e pageSize")
        .WithOpenApi()
        .Produces<IEnumerable<UserResponseDto>>(StatusCodes.Status200OK)
        .Produces<PagedResult<UserResponseDto>>(StatusCodes.Status200OK)
        .Produces<object>(StatusCodes.Status400BadRequest);
    }
}


