namespace DtedLibrary
{
    public class UserHeaderLabel
    {
        public Coordinate Longitude { get; }
        public Coordinate Latitude { get; }
        public float LatitudeIntervalSeconds { get; }
        public float LongitudeIntervalSeconds { get; }
        public int LongitudeLinesCount { get; }
        public int LatitudeLinesCount { get; }

        public int BinarySize => 80;

        public UserHeaderLabel(byte[] buffer, int offset)
        {
            Longitude = Coordinate.Parse(buffer, offset + 4, "DDDMMSSH");
            Latitude = Coordinate.Parse(buffer, offset + 12, "DDDMMSSH");
            LatitudeIntervalSeconds = Utilities.ReadFloat(buffer, 20, offset + 4, 3);
            LongitudeIntervalSeconds = Utilities.ReadFloat(buffer, 24, offset + 4, 3);

            LongitudeLinesCount = Utilities.ReadInt(buffer, 47, offset + 4);
            LatitudeLinesCount = Utilities.ReadInt(buffer, 51, offset + 4);
        }
    }
}