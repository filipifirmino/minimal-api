using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.Mappers;
using minimal_api.Dominio.Services;
using minimal_api.Infra.Repository;

namespace minimal_api.Dominio.UseCases;

public class CreateUserProcess(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher) : ICreateUserProcess
{
    public async Task<UserResponseDto> HandleAsync(CreateUserRequestDto request)
    {
        var hashedPassword = passwordHasher.HashPassword(request.Password);
        var user = request.ToDomain(hashedPassword);
        var userEntity = user.ToEntity();
        
        var createdUser = await userRepository.AddAsync(userEntity);
        
        return createdUser!.ToResponseDto();
    }
}