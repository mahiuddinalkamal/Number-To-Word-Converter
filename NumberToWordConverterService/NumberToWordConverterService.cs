using System;
using System.Linq;

namespace NumberToWordConverterService
{
    public class NumberToWordConverterService : INumberToWordConverterService
    {
        private bool validateInput(string numAsString)
        {
            // Check if the input is empty
            if (numAsString.Length == 0) return false;

            // Check if number only
            string digitOnly = numAsString.Replace("-", string.Empty);
            digitOnly = digitOnly.Replace(",", string.Empty);
            foreach (char c in digitOnly)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            // If the input is negative
            if (numAsString.Contains("-"))
            {
                // If mimus sign is not at the start
                if (!numAsString.StartsWith("-")) return false;

                // If only the minus is the input
                if (numAsString.Length == 1) return false;

                // If more than one minus in the input
                var count = numAsString.Count(x => x == '-');
                if (count > 1) return false;

                // If only zero is present along with minus in the input
                if (!numAsString.Contains(","))
                {
                    if (Convert.ToInt64(numAsString) == 0) return false;
                }
                else
                {
                    string inputWithoutComma = numAsString.Replace(',', '.');
                    if (Convert.ToDouble(inputWithoutComma) == 0.0) return false;
                }
            }

            // If the input contains decimals
            if (numAsString.Contains(","))
            {
                // If only "," in the input
                if (numAsString.Length == 1) return false;
                
                // If more than one comma in the input
                var count = numAsString.Count(x => x == ',');
                if (count > 1) return false;

                // Spliting input into integer and decimal part
                char[] separator = { ',' };
                string[] splitNum = numAsString.Split(separator);
                string integerAsString = splitNum[0];

                // Blocking any number beyond maximum allowed dollars
                string integerAsStringWithoutLeadingZeros = removeLeadingZeros(integerAsString);
                if (integerAsStringWithoutLeadingZeros.Length > 9) return false; 
                
                // Blocking any number beyond maximum allowed cents
                if (splitNum.Length > 1)
                {
                    string decimalAsString = splitNum[1];
                    if (decimalAsString.Length > 2) return false;
                }
            }
            else
            {
                // Case: When the input is integer only
                // Blocking any number beyond maximum allowed dollars
                string integerAsStringWithoutLeadingZeros = removeLeadingZeros(numAsString);
                if (integerAsStringWithoutLeadingZeros.Length > 9) return false;
            }

            return true;
        }
        private static string removeWhitespace(string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }
        private static string removeLeadingZeros(string input)
        {
            return input.TrimStart(new Char[] { '0' });
        }
        public string getNumber(string numAsString)
        {
            // Pre-process input data
            numAsString = removeWhitespace(numAsString);

            // Checking input validation
            if (!validateInput(numAsString))
                return "Invalid Input! Please try again.";

            // Spliting input in two parts (integer and decimal)
            char[] separator = { ',' };
            string[] splitNum = numAsString.Split(separator);

            /* I allowed any number starting without
            an integer,it can be changed depending on the semantics.*/
            string integerAsString = splitNum[0];
            if (integerAsString.Length == 0 || integerAsString == "-") integerAsString += "0";
            
            // Converting integer number to words
            int integerNumber = Convert.ToInt32(integerAsString); // minus can be omitted in the case like -0,1
            string words = NumberToWords(integerNumber);

            //adding the minus in case it ommited due to conversion
            if(integerAsString.Contains("-"))
            {
                if (!words.Contains("minus"))
                    words = "minus " + words;
            }
            words = words.Trim();
            
            if (integerNumber == 1) words += " dollar"; // for one dollar
            else words += " dollars"; // for any other amount of dollars

            if (splitNum.Length > 1)
            {
                string decimalAsString = splitNum[1];

                // Padding "0" with single digit decimal
                if (decimalAsString.Length == 1)
                {
                    if (!decimalAsString.StartsWith("0"))
                        decimalAsString += "0";
                }

                // Converting decimal number to words
                int decimalNumber = Convert.ToInt32(decimalAsString);
                words += " and " + NumberToWords(decimalNumber);

                if (decimalAsString == "01") words += " cent"; // for one cent
                else words += " cents"; // for any other amount of cents
            }

            return words;
        }
        private static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }
    }
}
