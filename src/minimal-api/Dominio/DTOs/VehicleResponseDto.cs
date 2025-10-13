namespace minimal_api.Dominio.DTOs;

public record VehicleResponseDto(
    Guid Id,
    string Brand,
    string Model,
    string Year,
    string Color,
    string LicensePlate
);


