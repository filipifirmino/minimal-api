using minimal_api.Dominio.Enums;

namespace minimal_api.Dominio.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Status Status { get; set; }

    public User(string name, string email, string password, Status status)
    {
        Name = name;
        Email = email;
        Password = password;
        Status = status;
        Id = Guid.NewGuid();
    }
    
    // Construtor sem parâmetros para EF Core
    protected User()
    {
    }
  
    public string GetInformationAboutUser()
    {
        return $"Name: {Name}, Email: {Email}, Status: {Status}";
    }
}


