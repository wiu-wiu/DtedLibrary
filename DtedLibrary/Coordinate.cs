using System;
using System.Linq;

namespace DtedLibrary
{
    public struct Coordinate
    {
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="formatString"> default format string is "DDDMMSSH" </param>
        /// <returns></returns>
        public static Coordinate Parse(byte[] buffer, int offset, string formatString = "DDDMMSSH")
        {
            var format = new ParsedFormat(formatString);

            var degree = Utilities.ReadInt(buffer, offset, format.DegreeLength);
            offset += format.DegreeLength;

            var minute = Utilities.ReadInt(buffer, offset, format.MinuteLength);
            offset += format.MinuteLength;

            float second;
            if (format.SecondsPointPosition == -1)
            {
                second = Utilities.ReadInt(buffer, offset, format.SecondsLength);
                offset += format.SecondsLength;
            }
            else
            {
                second = Utilities.ReadFloat(buffer, offset, format.SecondsLength, format.SecondsPointPosition);
                offset += format.SecondsLength + 1;
            }
            if (format.HemisphereLength != 1)
            {
                throw new Exception("Format error! Only one letter hemisphere is supported!");
            }
            var hemisphere = (Hemisphere) buffer[offset];

            return new Coordinate(degree, minute, second, hemisphere);
        }

        public Coordinate(int degree, int minute, float second, Hemisphere hemisphere) : this()
        {
            Degree = degree;
            Minute = minute;
            Second = second;
            Hemisphere = hemisphere;
        }

        public int BinarySize => 8;

        public int Degree { get; }
        public int Minute { get; }
        public float Second { get; }
        public Hemisphere Hemisphere { get; }

        private struct ParsedFormat
        {
            public readonly int DegreeLength;
            public readonly int MinuteLength;
            public readonly int SecondsLength;
            public readonly int SecondsPointPosition;
            public readonly int HemisphereLength;

            // we hope that all formats parts are placed in following order D, M, S, H
            public ParsedFormat(string formatString)
            {
                DegreeLength = formatString.Count(c => c == 'D');
                MinuteLength = formatString.Count(c => c == 'M');
                SecondsLength = formatString.Count(c => c == 'S');
                HemisphereLength = formatString.Count(c => c == 'H');

                var secondsStartPosition = formatString.IndexOf('S');
                var pointPosition = formatString.IndexOf('.');

                SecondsPointPosition = pointPosition == -1 ? -1 : pointPosition - secondsStartPosition;
            }
        }
    }
}