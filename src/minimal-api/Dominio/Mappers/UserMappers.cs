using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.Entities;
using minimal_api.Infra.Entities;

namespace minimal_api.Dominio.Mappers;

public static class UserMappers
{
    public static User ToDomain(this UserEntity entity)
     => new User(entity.Name, entity.Email, entity.Password, entity.Status)
     {
         Id = entity.Id
     };

    public static UserEntity ToEntity(this User domain)
        => new UserEntity
        {
            Id = domain.Id,
            Email = domain.Email,
            Name = domain.Name,
            Password = domain.Password,
            Status = domain.Status
        };

    public static UserResponseDto ToResponseDto(this UserEntity entity)
        => new UserResponseDto(
            entity.Id,
            entity.Name,
            entity.Email,
            entity.Status
        );

    public static UserResponseDto ToResponseDto(this User domain)
        => new UserResponseDto(
            domain.Id,
            domain.Name,
            domain.Email,
            domain.Status
        );

    public static User ToDomain(this CreateUserRequestDto dto, string hashedPassword)
        => new User(dto.Name, dto.Email, hashedPassword, Dominio.Enums.Status.Active);

    public static UserEntity ToEntity(this UpdateUserRequestDto dto, string currentPassword)
        => new UserEntity
        {
            Id = dto.Id,
            Name = dto.Name,
            Email = dto.Email,
            Status = dto.Status,
            Password = currentPassword
        };
}