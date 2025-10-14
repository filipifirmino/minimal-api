using minimal_api.Dominio.DTOs;

namespace minimal_api.Dominio.UseCases;

public interface ILoginProcess
{
    Task<LoginResponseDto?> HandleAsync(LoginRequestDto request);
}

