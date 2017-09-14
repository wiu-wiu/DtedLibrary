namespace DtedLibrary
{
    public static class Utilities
    {
        public static int ReadInt(byte[] buffer, int offset, int byteCount)
        {
            var result = 0;
            for (var i = 0; i < byteCount; ++i)
            {
                var digit = buffer[offset + i] - '0';
                result = result * 10 + digit;
            }
            return result;
        }

        public static float ReadFloat(byte[] buffer, int offset, int byteCount, int pointPosition)
        {
            // float could be in two formats: NNNN or NNN.NN.
            // it means that there could not be actual point character
            // but we should consider it with respect to pointPosition argument.
            // the other option is that sequence contains point character
            // for example args are: byteCount = 4, pointPosition = 3
            // and characters are 123.4. so it actually 5 bytes.
            // therefore we should skip point character

            var result = 0f;
            for (var i = 0; i < byteCount; ++i)
            {
                var character = buffer[offset + i];
                if (character == '.')
                {
                    ++byteCount;
                    ++pointPosition;
                    continue;
                }
                var digit = character - '0';
                result = result * 10 + digit;
            }
            for (var i = pointPosition; i < byteCount; ++i)
            {
                result /= 10;
            }
            return result;
        }

        public static string ReadString(byte[] buffer, int offset, int byteCount)
        {
            var chars = new char[byteCount];
            for (var i = 0; i < byteCount; ++i)
            {
                chars[i] = (char) buffer[offset + i];
            }
            return new string(chars);
        }
    }
}