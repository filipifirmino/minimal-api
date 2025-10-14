namespace minimal_api.Dominio.Entities;

public class Vehicle
{
    public Guid Id { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Year { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string LicensePlate { get; set; } = string.Empty;
    
    public Vehicle(string brand, string model, string year, string color, string plate)
    {
        Brand = brand;
        Model = model;
        Year = year;
        Color = color;
        LicensePlate = plate;
        Id = Guid.NewGuid();
    }
    
    // Construtor sem parâmetros para EF Core
    protected Vehicle()
    {
    }
    
    public string GetInformationAboutVehicle()
    {
        return $"Brand: {Brand}, Model: {Model}, Year: {Year}, Color: {Color}, Plate: {LicensePlate}";
    }
}



