using FluentAssertions;
using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.Validators;

namespace minimal_api.Tests.Validators;

public class CreateUserRequestValidatorTests
{
    private readonly CreateUserRequestValidator _validator = new();

    [Fact]
    public async Task Validator_ShouldPass_WhenRequestIsValid()
    {
        var request = new CreateUserRequestDto("João Silva", "joao@exemplo.com", "senha123");

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("", "joao@exemplo.com", "senha123")]
    [InlineData("Jo", "joao@exemplo.com", "senha123")]
    [InlineData(null, "joao@exemplo.com", "senha123")]
    public async Task Validator_ShouldFail_WhenNameIsInvalid(string? name, string email, string password)
    {
        var request = new CreateUserRequestDto(name!, email, password);

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Theory]
    [InlineData("João Silva", "", "senha123")]
    [InlineData("João Silva", "emailinvalido", "senha123")]
    [InlineData("João Silva", null, "senha123")]
    public async Task Validator_ShouldFail_WhenEmailIsInvalid(string name, string? email, string password)
    {
        var request = new CreateUserRequestDto(name, email!, password);

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Theory]
    [InlineData("João Silva", "joao@exemplo.com", "")]
    [InlineData("João Silva", "joao@exemplo.com", "12345")]
    [InlineData("João Silva", "joao@exemplo.com", null)]
    public async Task Validator_ShouldFail_WhenPasswordIsInvalid(string name, string email, string? password)
    {
        var request = new CreateUserRequestDto(name, email, password!);

        var result = await _validator.ValidateAsync(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Password");
    }
}

