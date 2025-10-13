using Microsoft.EntityFrameworkCore;
using minimal_api.Infra.Context;
using minimal_api.Infra.Entities;

namespace minimal_api.Infra.Repository;

public class UserRepository(DataContext dbContext) : RepositoryBase<UserEntity>(dbContext), IUserRepository
{
    
}