namespace minimal_api.Dominio.DTOs;

public record CreateUserRequestDto(
    string Name,
    string Email,
    string Password
);


