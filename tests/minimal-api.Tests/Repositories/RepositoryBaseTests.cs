using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.Enums;
using minimal_api.Infra.Context;
using minimal_api.Infra.Entities;
using minimal_api.Infra.Repository;

namespace minimal_api.Tests.Repositories;

public class RepositoryBaseTests : IDisposable
{
    private readonly DataContext _context;
    private readonly RepositoryBase<UserEntity> _repository;

    public RepositoryBaseTests()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new DataContext(options);
        _repository = new RepositoryBase<UserEntity>(_context);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllEntities()
    {

        var user1 = new UserEntity { Id = Guid.NewGuid(), Name = "User 1", Email = "user1@test.com", Password = "hash1", Status = Status.Active };
        var user2 = new UserEntity { Id = Guid.NewGuid(), Name = "User 2", Email = "user2@test.com", Password = "hash2", Status = Status.Active };
        
        await _context.Users.AddRangeAsync(user1, user2);
        await _context.SaveChangesAsync();


        var result = await _repository.GetAllAsync();


        result.Should().HaveCount(2);
        result.Should().Contain(u => u.Email == "user1@test.com");
        result.Should().Contain(u => u.Email == "user2@test.com");
    }

    [Fact]
    public async Task GetPagedAsync_ShouldReturnPagedResults()
    {

        for (int i = 1; i <= 25; i++)
        {
            await _context.Users.AddAsync(new UserEntity
            {
                Id = Guid.NewGuid(),
                Name = $"User {i}",
                Email = $"user{i}@test.com",
                Password = $"hash{i}",
                Status = Status.Active
            });
        }
        await _context.SaveChangesAsync();

        var paginationParams = new PaginationParameters
        {
            PageNumber = 2,
            PageSize = 10
        };


        var result = await _repository.GetPagedAsync(paginationParams);


        result.Should().NotBeNull();
        result.Items.Should().HaveCount(10);
        result.TotalCount.Should().Be(25);
        result.PageNumber.Should().Be(2);
        result.PageSize.Should().Be(10);
        result.TotalPages.Should().Be(3);
        result.HasPrevious.Should().BeTrue();
        result.HasNext.Should().BeTrue();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEntity_WhenEntityExists()
    {

        var userId = Guid.NewGuid();
        var user = new UserEntity { Id = userId, Name = "Test User", Email = "test@test.com", Password = "hash", Status = Status.Active };
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();


        var result = await _repository.GetByIdAsync(userId);


        result.Should().NotBeNull();
        result!.Id.Should().Be(userId);
        result.Email.Should().Be("test@test.com");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenEntityDoesNotExist()
    {

        var nonExistentId = Guid.NewGuid();


        var result = await _repository.GetByIdAsync(nonExistentId);


        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_ShouldAddEntity()
    {

        var user = new UserEntity { Id = Guid.NewGuid(), Name = "New User", Email = "new@test.com", Password = "hash", Status = Status.Active };


        var result = await _repository.AddAsync(user);


        result.Should().NotBeNull();
        result!.Email.Should().Be("new@test.com");

        var savedUser = await _context.Users.FindAsync(user.Id);
        savedUser.Should().NotBeNull();
        savedUser!.Email.Should().Be("new@test.com");
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {

        var user = new UserEntity { Id = Guid.NewGuid(), Name = "Original Name", Email = "test@test.com", Password = "hash", Status = Status.Active };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        _context.Entry(user).State = EntityState.Detached;

        user.Name = "Updated Name";


        await _repository.UpdateAsync(user);


        var updatedUser = await _context.Users.FindAsync(user.Id);
        updatedUser.Should().NotBeNull();
        updatedUser!.Name.Should().Be("Updated Name");
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntity()
    {

        var user = new UserEntity { Id = Guid.NewGuid(), Name = "To Delete", Email = "delete@test.com", Password = "hash", Status = Status.Active };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();


        await _repository.DeleteAsync(user);


        var deletedUser = await _context.Users.FindAsync(user.Id);
        deletedUser.Should().BeNull();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}

