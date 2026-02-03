using Microsoft.EntityFrameworkCore;
using minimal_api.Infra.Context;
using minimal_api.Infra.Entities;
using minimal_api.Infra.Repository.Interface;

namespace minimal_api.Infra.Repository;

public class UserRepository(DataContext dbContext) : RepositoryBase<UserEntity>(dbContext), IUserRepository
{
    
}