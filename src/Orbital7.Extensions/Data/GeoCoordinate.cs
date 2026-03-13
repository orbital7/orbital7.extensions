namespace Orbital7.Extensions.Data;

public record GeoCoordinate
{
    public required double Latitude { get; init; }

    public required double Longitude { get; init; }
}
