namespace DtedLibrary
{
    public struct CoordinatePoint
    {
        public Coordinate Latitude { get; }
        public Coordinate Longitude { get; }

        public CoordinatePoint(Coordinate latitude, Coordinate longitude) : this()
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public static CoordinatePoint Parse(
            byte[] buffer, int latitudeOffset, int longitudeOffset,
            string latitudeFormat = "DDDMMSSH", string longitudeFormat = "DDDMMSSH")
        {
            var latitude = Coordinate.Parse(buffer, latitudeOffset, latitudeFormat);
            var longitude = Coordinate.Parse(buffer, longitudeOffset, longitudeFormat);

            return new CoordinatePoint(latitude, longitude);
        }
    }
}