namespace DtedLibrary
{
    public partial class Dted
    {
        public struct ColorArgb
        {
            public byte B;
            public byte G;
            public byte R;
            public byte A;

            public static readonly ColorArgb Black = new ColorArgb(255, 0, 0, 0);
            public static readonly ColorArgb White = new ColorArgb(255, 255, 255, 255);
            public static readonly ColorArgb Transparent = new ColorArgb(0, 0, 0, 0);

            public ColorArgb(byte a, byte r, byte g, byte b)
            {
                A = a;
                R = r;
                G = g;
                B = b;
            }
        }

    }
}
