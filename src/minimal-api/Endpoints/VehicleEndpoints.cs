using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.Mappers;
using minimal_api.Infra.Repository;
using minimal_api.Infra.Entities;

namespace minimal_api.Endpoints;

public static class VehicleEndpoints
{
    public static void MapVehicleEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/vehicles").WithTags("Vehicles");

        group.MapGet("/", async (IRepositoryBase<VehicleEntity> vehicleRepository, int? pageNumber, int? pageSize) =>
        {
            try
            {
                if (!pageNumber.HasValue && !pageSize.HasValue)
                {
                    var vehicles = await vehicleRepository.GetAllAsync();
                    var vehiclesDto = vehicles.Select(v => v.ToResponseDto());
                    return Results.Ok(vehiclesDto);
                }

                var pagination = new PaginationParameters
                {
                    PageNumber = pageNumber ?? 1,
                    PageSize = pageSize ?? 10
                };

                var pagedVehicles = await vehicleRepository.GetPagedAsync(pagination);
                var pagedResponse = new PagedResult<VehicleResponseDto>(
                    pagedVehicles.Items.Select(v => v.ToResponseDto()),
                    pagedVehicles.TotalCount,
                    pagedVehicles.PageNumber,
                    pagedVehicles.PageSize
                );

                return Results.Ok(pagedResponse);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        })
        .WithName("GetAllVehicles")
        .WithSummary("Listar todos os veículos")
        .WithDescription("Retorna uma lista com todos os veículos cadastrados no sistema. Suporta paginação com parâmetros pageNumber e pageSize")
        .WithOpenApi()
        .Produces<IEnumerable<VehicleResponseDto>>(StatusCodes.Status200OK)
        .Produces<PagedResult<VehicleResponseDto>>(StatusCodes.Status200OK)
        .Produces<object>(StatusCodes.Status400BadRequest);

        group.MapGet("/{id:guid}", async (Guid id, IRepositoryBase<VehicleEntity> vehicleRepository) =>
        {
            try
            {
                var vehicle = await vehicleRepository.GetByIdAsync(id);
                if (vehicle == null)
                    return Results.NotFound(new { error = "Vehicle not found" });

                return Results.Ok(vehicle.ToResponseDto());
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        })
        .WithName("GetVehicle")
        .WithSummary("Buscar veículo por ID")
        .WithDescription("Retorna os dados de um veículo específico pelo seu ID")
        .WithOpenApi()
        .Produces<VehicleResponseDto>(StatusCodes.Status200OK)
        .Produces<object>(StatusCodes.Status400BadRequest)
        .Produces<object>(StatusCodes.Status404NotFound);

        group.MapPost("/", async (
            [FromBody] CreateVehicleRequestDto request, 
            IRepositoryBase<VehicleEntity> vehicleRepository,
            IValidator<CreateVehicleRequestDto> validator) =>
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
                var vehicle = request.ToDomain();
                var vehicleEntity = vehicle.ToEntity();
                var createdVehicle = await vehicleRepository.AddAsync(vehicleEntity);

                return Results.Created($"/api/vehicles/{createdVehicle!.Id}", createdVehicle.ToResponseDto());
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        })
        .WithName("CreateVehicle")
        .WithSummary("Criar novo veículo")
        .WithDescription("Cadastra um novo veículo no sistema")
        .WithOpenApi()
        .Produces<VehicleResponseDto>(StatusCodes.Status201Created)
        .Produces<object>(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id:guid}", async (Guid id, IRepositoryBase<VehicleEntity> vehicleRepository) =>
        {
            try
            {
                var vehicle = await vehicleRepository.GetByIdAsync(id);
                if (vehicle == null)
                    return Results.NotFound(new { error = "Vehicle not found" });

                await vehicleRepository.DeleteAsync(vehicle);
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        })
        .WithName("DeleteVehicle")
        .WithSummary("Deletar veículo")
        .WithDescription("Remove permanentemente um veículo do sistema (requer autenticação)")
        .RequireAuthorization()
        .WithOpenApi()
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces<object>(StatusCodes.Status400BadRequest)
        .Produces<object>(StatusCodes.Status404NotFound);
    }
}


