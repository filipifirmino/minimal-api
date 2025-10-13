namespace minimal_api.Dominio.DTOs;

public record LoginResponseDto(
    string Token,
    UserResponseDto User
);


