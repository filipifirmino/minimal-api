using Microsoft.EntityFrameworkCore;
using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.Mappers;
using minimal_api.Dominio.Services;
using minimal_api.Infra.Repository;

namespace minimal_api.Dominio.UseCases;

public class LoginProcess(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher) : ILoginProcess
{
    public async Task<LoginResponseDto?> HandleAsync(LoginRequestDto request)
    {
        var users = await userRepository.GetAllAsync();
        var user = users.FirstOrDefault(u => u.Email == request.Email);
        
        if (user == null)
            return null;
            
        var isValidPassword = passwordHasher.VerifyPassword(request.Password, user.Password);
        
        if (!isValidPassword)
            return null;
            
        // Por enquanto retornando um token simples
        // Em produção, implementar JWT adequadamente
        var token = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{user.Email}:{DateTime.UtcNow}"));
        
        return new LoginResponseDto(token, user.ToResponseDto());
    }
}




