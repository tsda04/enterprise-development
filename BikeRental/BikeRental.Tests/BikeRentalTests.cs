using BikeRental.Domain.Enum;

namespace BikeRental.Tests;

/// <summary>
/// Class for unit-tests
/// </summary>
public class BikeRentalTests(RentalFixture fixture) : IClassFixture<RentalFixture>
{
    
    /// <summary>
    /// Displays information about all sports bikes
    /// </summary>
    [Fact]
    public void InfoAboutSportBikes()
    {
        var expected = new List<int> {3, 6, 9};

        var actual = fixture.Bikes
            .Where(b => b.Model.Type == BikeType.Sport)
            .Select(b => b.Id)
            .ToList();

        Assert.Equal(expected, actual);
    }
    
    /// <summary>
    /// Displays the top 5 bike models ranked by rental revenue
    /// </summary>
    [Fact]
    public void TopFiveModelsIncome()
    {
        var expected = new List<int> {5, 8, 6, 2, 9};

        var actual = fixture.Lease
            .GroupBy(lease => lease.Bike.Model.Id)
            .Select(modelsGroup => new
            {
                ModelId = modelsGroup.Key,
                SumOfIncomes = modelsGroup.Sum(lease => lease.Bike.Model.RentPrice * lease.RentalDuration)
            })
            .OrderByDescending(models => models.SumOfIncomes)
            .Select(models => models.ModelId)
            .Take(5)
            .ToList();
        Assert.Equal(expected, actual);
    }
    
    /// <summary>
    /// Displays the top 5 bike models ranked by rental duration
    /// </summary
    [Fact]
    public void TopFiveModelsDuration()
    {
        var expected = new List<int> {5, 8, 6, 2, 9}; 

        var actual = fixture.Lease
            .GroupBy(lease => lease.Bike.Model.Id)
            .Select(modelsGroup => new
            {
                ModelId = modelsGroup.Key,
                SumOfDurations = modelsGroup.Sum(lease => lease.RentalDuration)
            })
            .OrderByDescending(models => models.SumOfDurations)
            .Select(models => models.ModelId)
            .Take(5)
            .ToList();

        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Displays information about the minimum, maximum, and average rental time
    /// </summary>
    [Fact]
    public void MinMaxAvgRental()
    {
        var expectedMinimum = 1;
        var expectedMaximum = 8;
        var expectedAverage = 4.3;
        
        var durations = fixture.Lease.Select(rent => rent.RentalDuration).ToList();
      
        Assert.Equal(expectedMinimum, durations.Min());
        Assert.Equal(expectedMaximum, durations.Max());
        Assert.Equal(expectedAverage, durations.Average());
    }
    
    
    /// <summary>
    ///  Displays the total rental time for each bike type
    /// </summary>
    [Theory]
    [InlineData(BikeType.Road, 11)]
    [InlineData(BikeType.Sport, 9)]
    [InlineData(BikeType.Mountain, 8)]
    [InlineData(BikeType.Hybrid, 15)]
    
    public void TotalRentalTimeByType(BikeType type, int expected)
    {
        var actual = fixture.Lease
            .Where(lease => lease.Bike.Model.Type == type)
            .Sum(lease => lease.RentalDuration);

        Assert.Equal(expected, actual);
    }
    
    /// <summary>
    ///   Displays information about customers who have rented bikes the most times
    /// </summary>
    [Fact]
    public void TopThreeRenters()
    {
        var expected = new List<int> {1, 2, 6};

        var actual = fixture.Lease
            .GroupBy(lease => lease.Renter.Id)
            .OrderByDescending(group => group.Count())
            .Select(group => group.Key)
            .Take(3)
            .ToList();

        Assert.Equal(expected, actual);
    }
}