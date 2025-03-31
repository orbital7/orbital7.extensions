namespace Orbital7.Extensions.Data;

public class GeoCoordinate
{
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }

    public GeoCoordinate(
        double latitude,
        double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
}
