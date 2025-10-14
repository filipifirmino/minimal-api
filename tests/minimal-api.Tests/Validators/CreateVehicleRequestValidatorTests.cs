using FluentAssertions;
using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.Validators;

namespace minimal_api.Tests.Validators;

public class CreateVehicleRequestValidatorTests
{
    private readonly CreateVehicleRequestValidator _validator = new();

    [Fact]
    public async Task Validator_ShouldPass_WhenRequestIsValid()
    {

        var request = new CreateVehicleRequestDto("Toyota", "Corolla", "2024", "Preto", "ABC-1234");


        var result = await _validator.ValidateAsync(request);


        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validator_ShouldPass_WhenLicensePlateIsMercosulFormat()
    {

        var request = new CreateVehicleRequestDto("Toyota", "Corolla", "2024", "Preto", "ABC1D23");


        var result = await _validator.ValidateAsync(request);


        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("", "Corolla", "2024", "Preto", "ABC-1234")]
    [InlineData(null, "Corolla", "2024", "Preto", "ABC-1234")]
    public async Task Validator_ShouldFail_WhenBrandIsInvalid(string? brand, string model, string year, string color, string plate)
    {

        var request = new CreateVehicleRequestDto(brand!, model, year, color, plate);


        var result = await _validator.ValidateAsync(request);


        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Brand");
    }

    [Theory]
    [InlineData("Toyota", "Corolla", "202", "Preto", "ABC-1234")]
    [InlineData("Toyota", "Corolla", "20244", "Preto", "ABC-1234")]
    [InlineData("Toyota", "Corolla", "1899", "Preto", "ABC-1234")]
    [InlineData("Toyota", "Corolla", "", "Preto", "ABC-1234")]
    public async Task Validator_ShouldFail_WhenYearIsInvalid(string brand, string model, string year, string color, string plate)
    {

        var request = new CreateVehicleRequestDto(brand, model, year, color, plate);


        var result = await _validator.ValidateAsync(request);


        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Year");
    }

    [Theory]
    [InlineData("Toyota", "Corolla", "2024", "Preto", "ABC12345")]
    [InlineData("Toyota", "Corolla", "2024", "Preto", "AB-1234")]
    [InlineData("Toyota", "Corolla", "2024", "Preto", "")]
    [InlineData("Toyota", "Corolla", "2024", "Preto", "123-4567")]
    public async Task Validator_ShouldFail_WhenLicensePlateIsInvalid(string brand, string model, string year, string color, string plate)
    {

        var request = new CreateVehicleRequestDto(brand, model, year, color, plate);


        var result = await _validator.ValidateAsync(request);


        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "LicensePlate");
    }
}

