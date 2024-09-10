namespace PNumberValidator;

public class PersonnummerValidator
{
    //define log file
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
            while (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Input cannot be empty. Please try again:");
                input = Console.ReadLine()?.Trim();
            }
            ValidateNumber(input);

        }
    }

    public static bool ValidateNumber(string input) {

        if (string.IsNullOrWhiteSpace(input))
        {
            Log("ERROR: Input cannot be empty.");
            return false;
        }
        // First check if it's a valid personal number
        if (IsValidPersonnummer(input))
        {
            Log($"{input}: Valid Swedish personal number (Personnummer or Samordningsnummer).");
            return true;
        }
        else
        {
            Log("Not a valid personal number, checking for valid organisation number.");
        }
        // If not a personal number, check if it's a valid organisation number
        if (IsValidOrganisationnummer(input))
        {
            Log($"{input}: Valid Swedish organisation number.");
            return true;
        }
        // If neither personal nor organisation number
        Log("Invalid number, neither personal nor organisation number.");
        return false;

    }
    private static void Log(string message)
    {
        message = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}\n";

        try { File.AppendAllText(LogFile, message); }
        catch (Exception e) { Console.WriteLine(e.Message); }

        Console.WriteLine(message);
    }
    private static bool IsValidPersonnummer(string pNummer)
    {
        // fist check for a '+' sign, which indicates the person is over 100 years old
        bool isOver100YearsOld = pNummer.Contains("+");
        // now remove any `-` or `+` signs
        pNummer = pNummer.Replace("-", "").Replace("+", "");

        // Check if resulting number is 10 or 12 digits
        if (pNummer.Length != 10 && pNummer.Length != 12)
        {
            Log($"ERROR: Invalid length for Personnummer: {pNummer.Length}");
            return false;
        }

        // Check if all characters are digits
        if (!pNummer.All(char.IsDigit))
        {
            Log($"ERROR: Personnummer contains non-digit characters: {pNummer}");
            return false;
        }

        //if no century given, calculate it
        if (pNummer.Length == 10)
        {
            //gets current year
            int currentYear = DateTime.Now.Year;
            //get current century
            int thisCentury = int.Parse(currentYear.ToString().Substring(0, 2));
            //calculate last century
            int lastCentury = thisCentury - 1;
            //if we already know the person is over 100 years old, and assuming less than 200 years old, we can assume the birth year is in the last century
            if (isOver100YearsOld)
            {
                //add previous century to the input
                pNummer = lastCentury + pNummer;
                Log($"Personnummer is over 100 years old, adjusting century to {lastCentury}00s");
            }
            //otherwise we need to determine the century of birth
            else
            {
                //caclulate the current YY
                int currentYY = currentYear % 100;
                //get YY of the input
                int inputYY = int.Parse(pNummer.Substring(0, 2));
                //if input YY is greater than current YY, the birth year is in the last century
                if (inputYY > currentYY)
                {
                    //add the first two digits of the current year to the input
                    pNummer = lastCentury + pNummer;
                    Log($"Personnummer is over {currentYY} years old, adjusting century to {lastCentury}00s");

                }
                //otherwise, the birth year is in the current century
                else
                {
                    //add the first two digits of the current year to the input
                    pNummer = thisCentury + pNummer;
                    Log($"Personnummer is under {currentYY} years old, adjusting century to {thisCentury}00s");

                }
            }
        }
        //check if the date is valid
        if (!IsValidDate(pNummer))
        {
            Log($"ERROR: Invalid date in Personnummer: {pNummer}");
            return false;
        }
        //now check the control number
        return IsValidLuhn(pNummer);
    }

    //Organisationsnummer består av tio siffror.
    //Den tredje siffran är alltid lägst en tvåa för att undvika förväxling med personnummer för fysiska personer
    private static bool IsValidOrganisationnummer(string oNummer)
    {
        oNummer = oNummer.Replace("-", "");

        // Check if resulting number is 10 or 12 digits
        if (oNummer.Length != 10 && oNummer.Length != 12)
        {
            Log($"ERROR: Invalid length for Organisationnummer: {oNummer.Length}");
            return false;
        }
        //if 12 digits (and the first two digits are '16'), reduce to leave a 10 digit number
        if (oNummer.Length == 12)
        {
            if (!oNummer.StartsWith("16"))
            {
                Log($"ERROR: Organisationnummer with invalid century prefix: {oNummer.Substring(0, 2)}");
                return false;
            }
            oNummer = oNummer.Substring(2);
        }
/*
        // The first digit indicates the type of organisation (not currently used)
        int type = int.Parse(oNummer.Substring(0, 1));
*/

        // The third and fourth digits must be 20 or higher
        if (int.Parse(oNummer.Substring(2, 2)) < 20)
        {
            Log("ERROR: The third and fourth digits of Organisationnummer must be at least 20.");
            return false;
        }
        // Validate with Luhn's algorithm
        return IsValidLuhn(oNummer);
    }

    /**
     * Check date is valid by trying to parse it from the pNummer
     **/
    private static bool IsValidDate(string pNummer)
    {
        // Check for null or empty string
        if (string.IsNullOrWhiteSpace(pNummer))
        {
            Log("ERROR: Date validation failed: input is null or empty.");
            return false;
        }
        // Should be 12 digits at this point
        if (pNummer.Length != 12)
        {
            Log($"ERROR: Date validation failed: Must include full date in yyyyMMDD format.");
            return false;
        }

        // Parse the day in case of samordningsnummer
        int day = int.Parse(pNummer.Substring(6, 2));
        // Adjust for samordningsnummer
        if (day >= 60)
        {
            day -= 60;
            Log($"Samordningsnummer detected. Adjusting day to {day}");
        }

        // now check if the date is valid
        return DateTime.TryParseExact($"{pNummer.Substring(0, 6)}{day:D2}", "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime result);
    }

    // Luhn's algorithm
    // discountiong century, multipy every other number (except control) with 1 or 2 and sum the digits
    // result deducted from mod 10 -> (10 - (result % 10)) % 10)
    // eg 811218-9876 -> 8 1 1 2 1 8 9 8 7 -> 1 6 1 2 2 2 8 1 8 8 1 4 -> sum = 44 -> mod neg 10 = 6 -> valid!
    private static bool IsValidLuhn(string number)
    {
        // Check for null or empty string
        if (string.IsNullOrWhiteSpace(number))
        {
            Log("ERROR: Luhn validation failed: input is null or empty.");
            return false;
        }
        // check 10 or 12 digits
        if (number.Length != 10 && number.Length != 12)
        {
            Log($"ERROR: Luhn validation failed: invalid length ({number.Length}).");
            return false;
        }

        //we need a 10 digit number to control
        if (number.Length == 12)
        {
            number = number.Substring(2);
        }

        //initialise sum
        int sum = 0;
        //track whether we are on an odd or even digit
        bool odd = true;
        //iterate through each digit in reverse, ignoring control digit
        for (int i = number.Length - 2; i >= 0; i--)
        {
            int n = int.Parse(number[i].ToString());
            if (odd)
            {
                n *= 2;
                //reduce to single digit if needed (eg 12 -> 3)
                if (n > 9)
                    n -= 9;
            }
            //add to sum
            sum += n;
            //toggle
            odd = !odd;
        }
        //calculate the Luhn digit
        int luhnDigit = (10 - (sum % 10)) % 10;
        //get the control digit
        int controlDigit = int.Parse(number[number.Length - 1].ToString());
        //now compare the control digit with the calculated Luhn digit
        return controlDigit == luhnDigit;
    }
}