using minimal_api.Dominio.DTOs;

namespace minimal_api.Dominio.UseCases;

public interface ICreateUserProcess
{
    Task<UserResponseDto> HandleAsync(CreateUserRequestDto request);
}