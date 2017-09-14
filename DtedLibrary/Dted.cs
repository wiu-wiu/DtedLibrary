using System.IO;

namespace DtedLibrary
{
    /// <summary>
    /// Digital Terrain Elevation Data
    /// </summary>
    public partial class Dted
    {
        /// <summary>
        /// User header label
        /// </summary>
        public UserHeaderLabel UserHeaderLabel { get; }
        
        /// <summary>
        /// Data set identification record
        /// </summary>
        public DatasetIdentification DsiRecord { get; }

        public Dataset Dataset { get; }

        public short MinElevation => Dataset.MinValue;
        public short MaxElevation => Dataset.MaxValue;

        public const short NullElevation = Dataset.NullValue;

        public Dted(UserHeaderLabel userHeaderLabel, DatasetIdentification dsiRecord, Dataset dataset)
        {
            UserHeaderLabel = userHeaderLabel;
            DsiRecord = dsiRecord;
            Dataset = dataset;
        }

        public ColorArgb[,] GenerateColorMatrix()
        {
            return GenerateColorMatrix(ColorArgb.Black, ColorArgb.White);
        }

        public ColorArgb[,] GenerateColorMatrix(ColorArgb minValueColor, ColorArgb maxValueColor)
        {
            return GenerateColorMatrix(minValueColor, maxValueColor, ColorArgb.Transparent);
        }
        public ColorArgb[,] GenerateColorMatrix(ColorArgb minValueColor, ColorArgb maxValueColor, ColorArgb nullValueColor)
        {
            var width = Dataset.LatitudeLinesCount;
            var height = Dataset.LongitudeLinesCount;

            var image = new ColorArgb[width, height];
            var valueRange = (float)(MaxElevation - MinElevation);

            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    image[i, j] = GetColor(Dataset.Elevations[i, j]);
                }
            }

            return image;

            ColorArgb GetColor(short elevation)
            {
                if (elevation == Dataset.NullValue)
                {
                    return nullValueColor;
                }
                var alpha = (elevation - MinElevation) / valueRange;

                var r = alpha * minValueColor.R + (1 - alpha) * maxValueColor.R;
                var g = alpha * minValueColor.G + (1 - alpha) * maxValueColor.G;
                var b = alpha * minValueColor.B + (1 - alpha) * maxValueColor.B;
                var a = alpha * minValueColor.A + (1 - alpha) * maxValueColor.A;

                return new ColorArgb((byte) a, (byte) r, (byte) g, (byte) b);
            }
        }

        public static Dted Parse(string filename)
        {
            var buffer = File.ReadAllBytes(filename);
            return Parse(buffer, 0);
        }

        public static Dted Parse(byte[] buffer, int offset)
        {
            var uhl = new UserHeaderLabel(buffer, offset);
            var dsiRecord = new DatasetIdentification(buffer, offset + 80);
            var dataset = new Dataset(buffer, offset + 80 + 648 + 2700, 
                dsiRecord.LongitudeLinesCount, 
                dsiRecord.LongitudeLinesCount);

            return new Dted(uhl, dsiRecord, dataset);
        }
    }
}
