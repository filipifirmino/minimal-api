using minimal_api.Dominio.Enums;

namespace minimal_api.Dominio.Entities;

public class Admin : User
{
    public Admin(string name, string email, string password, Status status) : base(name, email, password, status)
    {
    }
    
    protected Admin() : base()
    {
    }
}


