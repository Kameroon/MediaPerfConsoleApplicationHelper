namespace Tools
{
    /// <summary>
    /// Custom parser class.
    /// </summary>
    public class Parser
    {
        /// <summary>
        /// Convert string to byte.
        /// </summary>
        /// <param name="input">Input string to be converted.</param>
        /// <param name="decimalSeparator">Decimal separator.</param>
        /// <returns></returns>
        public static byte ToByte(string input, char decimalSeparator = char.MinValue)
        {
            return (byte)ToInt32(input, decimalSeparator);
        }

        /// <summary>
        /// Convert string to Int16.
        /// </summary>
        /// <param name="input">Input string to be converted.</param>
        /// <param name="decimalSeparator">Decimal separator.</param>
        /// <returns></returns>
        public static short ToInt16(string input, char decimalSeparator = char.MinValue)
        {
            return (short)ToInt32(input, decimalSeparator);
        }

        /// <summary>
        /// Convert string to Int32.
        /// </summary>
        /// <param name="input">Input string to be converted.</param>
        /// <param name="decimalSeparator">Decimal separator.</param>
        /// <returns></returns>
        public static int ToInt32(string input, char decimalSeparator = char.MinValue)
        {
            // - Return zero if input string is null -
            if (input == null)
            {
                return 0;
            }
            else
            {
                int n = 0;
                bool isNegative = false;
                bool separatorFound = false;

                for (int k = 0; k < input.Length; k++)
                {
                    char c = input[k];

                    if (c >= '0' && c <= '9' && separatorFound == false)
                    {
                        n = n * 10 + (c - '0');
                    }

                    else if (c == decimalSeparator)
                    {
                        separatorFound = true;
                    }

                    else if (c == '-')
                    {
                        isNegative = true;
                    }
                }

                return isNegative ? -n : n;
            }
        }

        /// <summary>
        /// Convert string to double.
        /// </summary>
        /// <param name="input">Input string to be converted.</param>
        /// <param name="decimalSeparator">Decimal separator.</param>
        /// <returns></returns>
        public static double ToDouble(string input, char decimalSeparator = char.MinValue)
        {
            return (double)ToDecimal(input, decimalSeparator);
        }

        /// <summary>
        /// Convert string to float (Single).
        /// </summary>
        /// <param name="input">Input string to be converted.</param>
        /// <param name="decimalSeparator">Decimal separator.</param>
        /// <returns></returns>
        public static float ToSingle(string input, char decimalSeparator = char.MinValue)
        {
            return (float)ToDecimal(input, decimalSeparator);
        }

        /// <summary>
        /// Convert string to decimal.
        /// </summary>
        /// <param name="input">Input string to be converted.</param>
        /// <param name="decimalSeparator">Decimal separator.</param>
        /// <returns></returns>
        public static decimal ToDecimal(string input, char decimalSeparator = char.MinValue)
        {
            // - Return zero if input string is null -
            if (input == null)
            {
                return 0m;
            }
            else
            {
                int n = 0;
                int validChars = 0;
                int scale = -1;
                bool isNegative = false;

                for (int k = 0; k < input.Length; k++)
                {
                    char c = input[k];

                    if (c >= '0' && c <= '9')
                    {
                        n = n * 10 + (c - '0');
                        validChars++;
                    }

                    else if (c == decimalSeparator)
                    {
                        scale = validChars;
                    }

                    else if (c == '-')
                    {
                        isNegative = true;
                    }
                }

                return new decimal(n, 0, 0, isNegative, (byte)(scale == -1 ? 0 : validChars - scale));
            }
        }
    }
}
