using System.IO;
using System.Text.RegularExpressions;

namespace TDSPaster
{
    class DataValidation
    {
        //check to make sure the file is a valid TDS file
        public static bool IsTdsFile(string fileLocation)
        {
            //get the file name without the extension
            string fileName = Path.GetFileNameWithoutExtension(fileLocation);
            //get the file path and remove the . in front
            string path = Path.GetExtension(fileLocation).Replace(".", "");
            //try to parse the string to see if it is an int
            int num;
            bool isNumeric = int.TryParse(path, out num);

            bool isTdsFormat = fileName.Trim().Length == 6;
            //has numeric file extension and 6 letter file-name
            if (isNumeric && isTdsFormat)
            {
                return true;
            }
            //not a TDS file
            return false;
        }

        //cleaning up the many types of comments that are not TDS compatable
        public static string DataValidator(string readingValue)
        {
            //check to make sure data already contains decimals. If not, continue on with validation. If so, checks and fixes decimal placement (ie data truncated from BEAM).
            /*if (readingValue.Contains(".") && readingValue.StartsWith("0"))
            {
                //This code checks to see if the reading has been truncated and repairs if it has
                decimal floatHolder;
                //executes if the reading has had just one zero truncated and checks for leading zero beam exports
                if (readingValue.StartsWith("0") && readingLength == 4)
                {
                    //adds a zero to the end of the reading by multiplying by the decimal
                    floatHolder = decimal.Parse(readingValue);
                    floatHolder = floatHolder * 1.0m;
                    readingValue = floatHolder.ToString();
                }
                //executes if the reading has had two zeroes truncated and checks for leading zero beam exports
                if (readingValue.StartsWith("0") && readingLength == 3)
                {
                    floatHolder = decimal.Parse(readingValue);
                    floatHolder = floatHolder * 1.00m;
                    readingValue = floatHolder.ToString();
                }
            }*/

            char[] charsToTrim = { '.' };//Place any characters that should not go into the TDS file here

            //create an array of TDS comments 
            var tdsComments = "+()/=<>;$[#*@?".ToCharArray();
            //checks if reading value contains comments
            var hasTdsComments = readingValue.IndexOfAny(tdsComments) != -1;
            //the following strings encase the numeric value
            string quote = "\"";
            string spaceQuote = " \"";
            string commentQuote = "\"   ";
            string singleZero = "0";
            string doubleZero = "00";
            
            //if the data is numbers with a single character the if statement will execute.           
            if (Regex.IsMatch(readingValue, @"\d"))
            {
                //todo put the trimming functions in their own method
                readingValue = readingValue.TrimStart('0'); //getting rid of any leading zeros
                readingValue = readingValue.Trim(charsToTrim);

                int readingLength = readingValue.Length;
                //check reading length and determine how to format the reading. If it is more than 4 or less than one, a blank reading is returned.
                switch (readingLength)
                {
                    case 4:
                        readingValue = quote + readingValue + quote;
                        break;
                    case 3:
                        readingValue = quote + readingValue + spaceQuote;
                        break;
                    case 2:
                        readingValue = quote + singleZero + readingValue + spaceQuote;
                        break;
                    case 1:
                        readingValue = quote + doubleZero + readingValue + spaceQuote;
                        break;
                    default:
                        readingValue = @"""    """;
                        break;
                }    
            }
            //check if the reading contains TDS comments.
            else if (hasTdsComments)
            {
                //remove all whitespace and format the comment for the TDS
                readingValue = readingValue.Replace(" ", string.Empty);
                readingValue = commentQuote + readingValue + quote;
            }
            //if the reading is not a valid reading or TDS comment replace reading with a blank reading
            else
            {
                readingValue = @"""    """;
            }
            return readingValue;
        }

        public static string RemoveNonAlpha(string line)
        {
            //Replace all multiple spaces with just one space, then remove the quotes.
            var newLine = Regex.Replace(line, @"\s+", " ");
            newLine = newLine.Replace("\"", string.Empty);
            return newLine;
        }
    }
}
