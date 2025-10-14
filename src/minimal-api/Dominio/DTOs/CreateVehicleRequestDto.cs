namespace minimal_api.Dominio.DTOs;

public record CreateVehicleRequestDto(
    string Brand,
    string Model,
    string Year,
    string Color,
    string LicensePlate
);



