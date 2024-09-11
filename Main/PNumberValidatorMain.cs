namespace PNumberValidator;

/// <summary>
/// Class to validate Swedish personal and organisation numbers.
/// </summary>
public class PersonnummerValidator
{
    private static readonly string LogFile = "history.log";

    private static void Main(string[] args)
    {
        //greeting
        Console.WriteLine("Welcome to the Swedish Personal and Organisation Number Validator!");
        Console.WriteLine();

        //main program loop
        while (true)
        {
            Console.WriteLine("Write a 10 or 12 digit number to validate, or type 'exit' to quit:");
            string? input = Console.ReadLine()?.Trim();

            // Exit the program if the user types 'exit'
            if (string.Equals(input, "exit", StringComparison.OrdinalIgnoreCase))
            {
                Log("Exiting program.");
                break;
            }
            // Validate input is not empty
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Input cannot be empty. Please try again:");
                continue;
            }
            ValidateNumber(input);
        }
    }

    /// <summary>
    /// Validates a 10 or 12 digit number to check if it's a valid Swedish personal or organisation number.
    /// </summary>
    /// <param name="input">number string</param>
    /// <returns></returns>
    public static bool ValidateNumber(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            Log("ERROR: Input is null or empty.");
            return false;
        }
        Log($"Validating: {input.Substring(0, Math.Min(input.Length, 13))}...");
        Console.WriteLine($"Validating: {input.Substring(0, Math.Min(input.Length, 13))}...");
        // First check if it's a valid personal number
        if (IsValidPersonnummer(input))
        {
            Log($"{input}: PASSED. Valid Swedish personal number (Personnummer or Samordningsnummer).");
            Console.WriteLine($"{input}: Valid Swedish personal number (Personnummer or Samordningsnummer).");
            return true;
        }
        else
        {
            Log($"{input}: Not valid personal number, checking if valid organisation number.");
        }
        // If not a personal number, check if it's a valid organisation number
        if (IsValidOrganisationnummer(input))
        {
            Log($"{input}: PASSED. Valid Swedish organisation number.");
            Console.WriteLine($"{input}: Valid Swedish organisation number.");

            return true;
        }
        // If neither personal nor organisation number
        Log($"{input}: Failed Validation");
        Console.WriteLine($"{input}: Failed validation.");
        return false;
    }

    /// <summary>
    /// Checks if valid Swedish personal number (Personnummer or Samordningsnummer)
    /// </summary>
    /// <param name="pNumber">number string</param>
    /// <returns></returns>
    private static bool IsValidPersonnummer(string pNumber)
    {
        // fist check for a '+' sign, which indicates the person is over 100 years old
        bool isOver100YearsOld = pNumber.Contains("+");

        // now remove any `-` or `+` signs
        pNumber = pNumber.Replace("-", "").Replace("+", "");

        // Check if resulting number is 10 or 12 digits
        if (pNumber.Length != 10 && pNumber.Length != 12)
        {
            Log($"ERROR: Invalid length: {pNumber.Length}");
            return false;
        }
        // Check if all remaining characters are digits
        if (!pNumber.All(char.IsDigit))
        {
            Log($"ERROR: Contains non-digit characters: {pNumber}");
            return false;
        }
        //if no century given, calculate it
        if (pNumber.Length == 10)
        {
            int currentYear = DateTime.Now.Year;
            int thisCentury = int.Parse(currentYear.ToString().Substring(0, 2));
            int lastCentury = thisCentury - 1;
            //if we already know the person is over 100 years old, and assuming less than 200 years old, we can assume the birth year is in the last century
            if (isOver100YearsOld)
            {
                //add previous century to the input
                pNumber = lastCentury + pNumber;
                Log($"Over 100 years old, adjusting century to {lastCentury}00s");
            }
            //otherwise we need to determine the century of birth
            else
            {
                int currentYY = currentYear % 100;
                int givenYY = int.Parse(pNumber.Substring(0, 2));
                //if given YY is greater than current YY, the birth year must the last century
                if (givenYY > currentYY)
                {
                    pNumber = lastCentury + pNumber;
                    Log($"Over {currentYY} years old, adjusting century to {lastCentury}00s");
                }
                //otherwise, the birth year is in the current century
                else
                {
                    //add the first two digits of the current year to the input
                    pNumber = thisCentury + pNumber;
                    Log($"Under {currentYY} years old, adjusting century to {thisCentury}00s");
                }
            }
        }
        //check if the date is valid
        if (!IsValidDate(pNumber))
        {
            Log($"ERROR: Invalid date in Personnummer: {pNumber}");
            return false;
        }
        //now check the control digit
        return IsValidLuhn(pNumber);
    }
    /// <summary>
    /// Checks if the date part of a personal number is a valid date
    /// </summary>
    /// <param name="pNumber">12 digit number string</param>
    /// <returns></returns>
    private static bool IsValidDate(string pNumber)
    {
        // Should be 12 digits at this point
        if (pNumber.Length != 12)
        {
            Log($"ERROR: Must include full date in yyyyMMdd format.");
            return false;
        }
        
        int year = int.Parse(pNumber.Substring(0, 4));
        int month = int.Parse(pNumber.Substring(4, 2));
        int day = int.Parse(pNumber.Substring(6, 2));
        // Adjust for samordningsnummer
        if (day > 60)
        {
            day -= 60;
            Log($"Samordningsnummer detected. Adjusting dd to {day}");
        }
        //simple checks for valid date including leap years
        if (month < 1 || month > 12) return false;
        if (day < 1 || day > DateTime.DaysInMonth(year, month)) return false;
        //check for future dates
        if (new DateTime(year, month, day) > DateTime.Now) return false;
        //date checks passed
        return true;
    }

    /// <summary>
    /// Checks if valid Swedish organisation number
    /// </summary>
    /// <param name="oNumber">number string</param>
    /// <returns></returns>
    private static bool IsValidOrganisationnummer(string oNumber)
    {
        //remove any `-` signs
        oNumber = oNumber.Replace("-", "");

        // Check if resulting number is 10 or 12 digits
        if (oNumber.Length != 10 && oNumber.Length != 12)
        {
            Log($"ERROR: Invalid length: {oNumber.Length}");
            return false;
        }
        // Check if all remaining characters are digits
        if (!oNumber.All(char.IsDigit))
        {
            Log($"ERROR: Contains non-digit characters: {oNumber}");
            return false;
        }
        //if 12 digits (and the first two digits are '16'), reduce to leave a 10 digit number
        if (oNumber.Length == 12)
        {
            if (!oNumber.StartsWith("16"))
            {
                Log($"ERROR: Invalid century prefix: {oNumber.Substring(0, 2)}");
                return false;
            }
            oNumber = oNumber.Substring(2);
        }
        // The third and fourth digits must be 20 or higher to be a valid organisation number
        if (int.Parse(oNumber.Substring(2, 2)) < 20)
        {
            Log("ERROR: The third and fourth digits of 10 digit organisation number must be at least 20.");
            return false;
        }
        //now check the control digit
        return IsValidLuhn(oNumber);
    }

    /// <summary>
    /// Validates control digit of a 10 digit number using Luhn's algorithm
    /// </summary>
    /// <param name="number">10 digit number string, excluding century</param>
    /// <returns></returns>
    private static bool IsValidLuhn(string number)
    {
        // check 10 or 12 digits
        if (number.Length != 10 && number.Length != 12)
        {
            Log($"ERROR: Invalid length ({number.Length}).");
            return false;
        }
        //we need a 10 digit number to control
        if (number.Length == 12)
        {
            number = number.Substring(2);
        }
        //initialise and track whether we are on an odd or even digit
        int sum = 0;
        bool odd = true;
        //iterate through each digit in reverse, ignoring control digit
        for (int i = number.Length - 2; i >= 0; i--)
        {
            int n = int.Parse(number[i].ToString());
            if (odd)
            {
                n *= 2;
                //reduce to single digit if needed (eg 12 -> 3)
                if (n > 9) n -= 9;
            }
            //add to sum and toggle odd/even
            sum += n;
            odd = !odd;
        }
        //calculate the Luhn digit
        int luhnDigit = (10 - (sum % 10)) % 10;
        //now compare the control digit with the calculated Luhn digit
        int controlDigit = int.Parse(number[number.Length - 1].ToString());
        if (controlDigit == luhnDigit)
        {
            return true;
        }
        else
        {
            Log($"ERROR: Invalid control digit. Expected {luhnDigit}, got {controlDigit}.");
            return false;
        }
    }

    /// <summary>
    /// Logs a message to the log file
    /// </summary>
    /// <param name="message"></param>
    private static void Log(string message)
    {
        message = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}\n";

        try { File.AppendAllText(LogFile, message); }
        catch (Exception e) { Console.WriteLine(e.Message); }
    }
}