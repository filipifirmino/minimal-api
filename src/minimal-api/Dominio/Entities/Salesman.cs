using minimal_api.Dominio.Enums;

namespace minimal_api.Dominio.Entities;

public class Salesman : User
{
    public Salesman(string name, string email, string password, Status status) : base(name, email, password, status)
    {
    }
    
    protected Salesman() : base()
    {
    }
}


