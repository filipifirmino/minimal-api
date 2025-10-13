using minimal_api.Dominio.Enums;

namespace minimal_api.Infra.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Status Status { get; set; }
}