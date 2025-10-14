using minimal_api.Dominio.Enums;

namespace minimal_api.Dominio.DTOs;

public record UpdateUserRequestDto(
    Guid Id,
    string Name,
    string Email,
    Status Status
);



