namespace BikeRental.Tests;

/// <summary>
/// A class just for print testing data
/// </summary>
public class TestDataPrinter
{
    public static void Main(string[] args)
    {
        var printer = new TestDataPrinter();
        printer.PrintDataCheck();
    }

    public void PrintDataCheck()
    {
        var fixture = new RentalFixture();
        
        Console.WriteLine("Модели");
        foreach (var model in fixture.Models)
        {
            Console.WriteLine("| {0,-40} | {1,-10} | {2,-10} | {3,-10} | {4,-10} | {5,-10} | {6,-5} | {7,-5} |", 
                model.Id, 
                model.Type, 
                model.WheelSize, 
                model.Weight, 
                model.MaxСyclistWeight, 
                model.BrakeType, 
                model.YearOfManufacture, 
                model.RentPrice);
        }

        Console.WriteLine("\n");
        Console.WriteLine("Велики");
        foreach (var bike in fixture.Bikes)
        {
            Console.WriteLine("| {0,-40} | {1,-10} | {2,-10} | {3,-20} |", 
                bike.Id, 
                bike.SerialNumber, 
                bike.Color, 
                bike.Model?.Id.ToString());
        }
        
        Console.WriteLine("\n");
        Console.WriteLine("Арендатели");
        foreach (var renter in fixture.Renters)
        {
            Console.WriteLine("| {0,-40} | {1,-20} | {2,-20} |", 
                renter.Id, 
                renter.FullName, 
                renter.PhoneNumber);
        }
        
        Console.WriteLine("\n");
        Console.WriteLine("Аренда");
        foreach (var rent in fixture.Lease)
        {
            Console.WriteLine("| {0,-40} | {1,-20} | {2,-20} | {3,-20} | {4,-5} |", 
                rent.Id, 
                rent.Bike?.Id.ToString(), 
                rent.Renter?.Id.ToString(), 
                rent.RentalStartTime.ToString("dd.MM.yyyy HH:mm"), 
                rent.RentalDuration);
        }
        
        Console.WriteLine($"Модели: {fixture.Models.Count} шт.");
        Console.WriteLine($"Арендаторы: {fixture.Renters.Count} шт.");
        Console.WriteLine($"Велосипеды: {fixture.Bikes.Count} шт.");
        Console.WriteLine($"Аренды: {fixture.Lease.Count} шт.");
    }
}