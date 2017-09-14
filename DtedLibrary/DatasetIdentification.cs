namespace DtedLibrary
{
    public class DatasetIdentification
    {
        public string HorizontalDatumCode { get; }

        public CoordinatePoint OriginPoint { get; }

        public CoordinatePoint SouthWestPoint { get; }

        public CoordinatePoint NorthWestPoint { get; }

        public CoordinatePoint NorthEastPoint { get; }

        public CoordinatePoint SouthEastPoint { get; }

        public float LatitudeIntervalSeconds { get; }

        public float LongitudeIntervalSeconds { get; }

        public int LatitudeLinesCount { get; }

        public int LongitudeLinesCount { get; }

        public int BinarySize => 648;

        public DatasetIdentification(byte[] buffer, int offset)
        {
            HorizontalDatumCode = Utilities.ReadString(buffer, offset + 144, 5);
            OriginPoint = CoordinatePoint.Parse(buffer,
                latitudeOffset: offset + 185,
                longitudeOffset: offset + 194,
                latitudeFormat: "DDMMSS.SH",
                longitudeFormat: "DDDMMSS.SH"
            );
            SouthWestPoint = CoordinatePoint.Parse(buffer,
                latitudeOffset: offset + 204,
                longitudeOffset: offset + 211,
                latitudeFormat: "DDMMSSH",
                longitudeFormat: "DDDMMSSH"
            );
            NorthWestPoint = CoordinatePoint.Parse(buffer,
                latitudeOffset: offset + 219,
                longitudeOffset: offset + 226,
                latitudeFormat: "DDMMSSH",
                longitudeFormat: "DDDMMSSH"
            );
            NorthEastPoint = CoordinatePoint.Parse(buffer,
                latitudeOffset: offset + 234,
                longitudeOffset: offset + 241,
                latitudeFormat: "DDMMSSH",
                longitudeFormat: "DDDMMSSH"
            );
            SouthEastPoint = CoordinatePoint.Parse(buffer,
                latitudeOffset: offset + 249,
                longitudeOffset: offset + 256,
                latitudeFormat: "DDMMSSH",
                longitudeFormat: "DDDMMSSH"
            );

            LatitudeIntervalSeconds = Utilities.ReadFloat(buffer, offset + 273, 4, 3);
            LongitudeIntervalSeconds = Utilities.ReadFloat(buffer, offset + 277, 4, 3);

            LatitudeLinesCount = Utilities.ReadInt(buffer, offset + 281, 4);
            LongitudeLinesCount = Utilities.ReadInt(buffer, offset + 285, 4);
        }

        public struct Date
        {
            public int Year { get; }
            public int Month { get; }

            public int BinarySize => 4;

            public Date(byte[] buffer, int offset)
            {
                Year = Utilities.ReadInt(buffer, offset, 2);
                Month = Utilities.ReadInt(buffer, offset + 2, 2);
            }

            /// <summary>
            /// Format is YY.MM
            /// </summary>
            public override string ToString()
            {
                return $"{Year}.{Month}";
            }
        }
    }
}