namespace DtedLibrary
{
    public class Dataset
    {
        public short[,] Elevations { get; }

        public const short NullValue = short.MinValue;

        public int LatitudeLinesCount { get; }

        public int LongitudeLinesCount { get; }

        public short MinValue { get; }

        public short MaxValue { get; }

        public Dataset(byte[] buffer, int offset, int latitudeCount, int longitudeCount)
        {
            LatitudeLinesCount = latitudeCount;
            LongitudeLinesCount = longitudeCount;

            Elevations = new short[latitudeCount, longitudeCount];
            var elevationsOffset = offset + 8;

            MinValue = NullValue;
            MaxValue = NullValue;

            for (var i = 0; i < latitudeCount; ++i)
            {
                for (var j = 0; j < longitudeCount; ++j)
                {
                    var elevation = ReadElevation(buffer, elevationsOffset + j * 2);
                    if (elevation != NullValue)
                    {
                        if (elevation < MinValue || MinValue == NullValue)
                        {
                            MinValue = elevation;
                        }
                        if (elevation > MaxValue || MaxValue == NullValue)
                        {
                            MaxValue = elevation;
                        }
                    }
                    Elevations[i, j] = elevation;
                }
                elevationsOffset += 12 + longitudeCount * 2;
            }
        }

        private static short ReadElevation(byte[] buffer, int offset)
        {
            if (buffer[offset] == 255 && buffer[offset + 1] == 255)
            {
                return NullValue;
            }
            var result = (short) ((buffer[offset] << 8) + (buffer[offset + 1] << 0));
            
            // Not surt if this if branch is needed - need more test data
            /*
            if (result < 0)
            {
                result = (short) ((1 << 15) & result);
                result *= -1;
            }*/
            return result;
        }
    }
}