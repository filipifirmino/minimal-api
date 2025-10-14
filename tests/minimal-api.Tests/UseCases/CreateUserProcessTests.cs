using FluentAssertions;
using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.Services;
using minimal_api.Dominio.UseCases;
using minimal_api.Infra.Entities;
using minimal_api.Infra.Repository;
using Moq;

namespace minimal_api.Tests.UseCases;

public class CreateUserProcessTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly CreateUserProcess _createUserProcess;

    public CreateUserProcessTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _createUserProcess = new CreateUserProcess(_userRepositoryMock.Object, _passwordHasherMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ShouldCreateUser_WhenRequestIsValid()
    {

        var request = new CreateUserRequestDto("João Silva", "joao@exemplo.com", "senha123");
        var hashedPassword = "hashedPassword123";
        var userId = Guid.NewGuid();

        _passwordHasherMock
            .Setup(x => x.HashPassword(request.Password))
            .Returns(hashedPassword);

        _userRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<UserEntity>()))
            .ReturnsAsync((UserEntity entity) =>
            {
                entity.Id = userId;
                return entity;
            });


        var result = await _createUserProcess.HandleAsync(request);


        result.Should().NotBeNull();
        result.Id.Should().Be(userId);
        result.Name.Should().Be(request.Name);
        result.Email.Should().Be(request.Email);

        _passwordHasherMock.Verify(x => x.HashPassword(request.Password), Times.Once);
        _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<UserEntity>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_ShouldHashPassword_BeforeStoringUser()
    {

        var request = new CreateUserRequestDto("João Silva", "joao@exemplo.com", "senha123");
        var hashedPassword = "hashedPassword123";

        _passwordHasherMock
            .Setup(x => x.HashPassword(request.Password))
            .Returns(hashedPassword);

        _userRepositoryMock
            .Setup(x => x.AddAsync(It.Is<UserEntity>(u => u.Password == hashedPassword)))
            .ReturnsAsync((UserEntity entity) => entity);


        await _createUserProcess.HandleAsync(request);


        _passwordHasherMock.Verify(x => x.HashPassword(request.Password), Times.Once);
        _userRepositoryMock.Verify(
            x => x.AddAsync(It.Is<UserEntity>(u => u.Password == hashedPassword)),
            Times.Once
        );
    }
}

