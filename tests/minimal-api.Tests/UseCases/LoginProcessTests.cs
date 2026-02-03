using FluentAssertions;
using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.Enums;
using minimal_api.Dominio.Services;
using minimal_api.Dominio.UseCases;
using minimal_api.Infra.Entities;
using minimal_api.Infra.Repository;
using minimal_api.Infra.Repository.Interface;
using Moq;

namespace minimal_api.Tests.UseCases;

public class LoginProcessTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly LoginProcess _loginProcess;

    public LoginProcessTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _loginProcess = new LoginProcess(_userRepositoryMock.Object, _passwordHasherMock.Object);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnToken_WhenCredentialsAreValid()
    {

        var request = new LoginRequestDto("joao@exemplo.com", "senha123");
        var userEntity = new UserEntity
        {
            Id = Guid.NewGuid(),
            Name = "João Silva",
            Email = request.Email,
            Password = "hashedPassword",
            Status = Status.Active
        };

        _userRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<UserEntity> { userEntity });

        _passwordHasherMock
            .Setup(x => x.VerifyPassword(request.Password, userEntity.Password))
            .Returns(true);


        var result = await _loginProcess.HandleAsync(request);


        result.Should().NotBeNull();
        result!.Token.Should().NotBeNullOrEmpty();
        result.User.Should().NotBeNull();
        result.User.Email.Should().Be(request.Email);

        _userRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        _passwordHasherMock.Verify(x => x.VerifyPassword(request.Password, userEntity.Password), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {

        var request = new LoginRequestDto("naoexiste@exemplo.com", "senha123");

        _userRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<UserEntity>());


        var result = await _loginProcess.HandleAsync(request);


        result.Should().BeNull();

        _userRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        _passwordHasherMock.Verify(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnNull_WhenPasswordIsInvalid()
    {

        var request = new LoginRequestDto("joao@exemplo.com", "senhaErrada");
        var userEntity = new UserEntity
        {
            Id = Guid.NewGuid(),
            Name = "João Silva",
            Email = request.Email,
            Password = "hashedPassword",
            Status = Status.Active
        };

        _userRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<UserEntity> { userEntity });

        _passwordHasherMock
            .Setup(x => x.VerifyPassword(request.Password, userEntity.Password))
            .Returns(false);


        var result = await _loginProcess.HandleAsync(request);


        result.Should().BeNull();

        _userRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        _passwordHasherMock.Verify(x => x.VerifyPassword(request.Password, userEntity.Password), Times.Once);
    }
}

