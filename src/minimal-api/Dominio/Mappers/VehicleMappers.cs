using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.Entities;
using minimal_api.Infra.Entities;

namespace minimal_api.Dominio.Mappers;

public static class VehicleMappers
{
    public static Vehicle ToDomain(this VehicleEntity entity)
        => new Vehicle(entity.Brand, entity.Model, entity.Year, entity.Color, entity.LicensePlate)
        {
            Id = entity.Id
        };

    public static VehicleEntity ToEntity(this Vehicle domain)
        => new VehicleEntity
        {
            Id = domain.Id,
            Brand = domain.Brand,
            Model = domain.Model,
            Year = domain.Year,
            Color = domain.Color,
            LicensePlate = domain.LicensePlate
        };

    public static VehicleResponseDto ToResponseDto(this VehicleEntity entity)
        => new VehicleResponseDto(
            entity.Id,
            entity.Brand,
            entity.Model,
            entity.Year,
            entity.Color,
            entity.LicensePlate
        );

    public static VehicleResponseDto ToResponseDto(this Vehicle domain)
        => new VehicleResponseDto(
            domain.Id,
            domain.Brand,
            domain.Model,
            domain.Year,
            domain.Color,
            domain.LicensePlate
        );

    public static Vehicle ToDomain(this CreateVehicleRequestDto dto)
        => new Vehicle(dto.Brand, dto.Model, dto.Year, dto.Color, dto.LicensePlate);
}


