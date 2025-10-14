using minimal_api.Dominio.Enums;

namespace minimal_api.Dominio.Entities;

public class Customer : User
{
    public Customer(string name, string email, string password, Status status) : base(name, email, password, status)
    {
    }
    
    protected Customer() : base()
    {
    }
}



