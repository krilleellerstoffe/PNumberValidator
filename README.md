
# Personal Number Validator

The `PersonnummerValidator` is a .NET C# application designed to validate Swedish personal numbers (Personnummer), coordination numbers (Samordningsnummer), and organization numbers (Organisationsnummer). This tool follows the rules and formats for these numbers as specified by Swedish authorities, including Skatteverket (the Swedish Tax Agency). The validator also handles historical and century-sensitive inputs, such as century adjustments for people over 100 years old.

## Features

- **Personnummer Validation**: Validates 10- or 12-digit Swedish personal identity numbers.
- **Samordningsnummer Validation**: Supports validation of Swedish coordination numbers, adjusting dates for the special 61–91 range.
- **Organisationsnummer Validation**: Validates Swedish organization numbers.
- **Luhn Algorithm**: Uses Luhn's algorithm to validate the control digit of the number.
- **Date Validation**: Ensures that a birth date is valid.

## How It Works

### Main Program

Upon starting the program, users are prompted to input a 10- or 12-digit number to validate. The program checks whether the input is:

- A valid **Personnummer** (personal number) or **Samordningsnummer** (coordination number)
- A valid **Organisationsnummer** (organization number)

If the input is valid, a message is printed confirming the type of number and its validity. If the input is invalid, an error message specifying the reason is logged.

### Validation Process

1. **Input Handling**: The program first checks for empty inputs or invalid characters (letters, spaces, special symbols).
2. **Length Check**: The input should be 10 or 12 digits after removing any hyphens (`-`) or plus signs (`+`).
3. **Century Handling**: For personal numbers, the program adjusts the century based on the input:
   - If a `+` is present, indicating the person is over 100 years old, the year is adjusted to the last century.
   - If no century is provided, the program infers the century based on the current year.
4. **Date Validation**: The program validates the date part of the number, ensuring it represents a valid date (e.g., leap year calculations for February 29).
5. **Luhn Check**: The last digit (control digit) is validated using Luhn's algorithm.
6. **Organisationsnummer Validation**: For organization numbers, the third digit must be 2 or higher, and the Luhn check is applied.

## Build scripts
Build scripts are provided which automatically checks if .net is installed, then builds, runs tests and main project.
Linux - `build-and-run-linux.sh`
Windows - `build-and-run-win.bat`

## Usage

To run the application:

1. Clone the repository and ensure the .NET SDK is installed on your machine.
2. Navigate to the solution directory and build the solution:
   ```bash
   dotnet build
   ```
3. Run the application:
   ```bash
   dotnet run --project </Main/PNumberValidator.csproj>
   ```


## Running Tests

To run the tests:

1. Navigate to the solution directory.
2. Build and execute the tests using the following command:
   ```bash
   dotnet test
   ```
## Logging

The application logs all validation actions to a file called `history.log` located in the build folder of the project <\Main\bin\Debug\net6.0>. This log contains timestamps and information about all valid and invalid attempts to validate personal, coordination, or organization numbers.

## Examples

- **Valid Personnummer**: `201701102384` (12 digits) or `141206-2380` (10 digits)
- **Valid Organisationsnummer**: `556614-3185` (10 digits) or `16556601-6399` (12 digits)
- **Valid Samordningsnummer**: `190910799824`

## License

This project is licensed under the MIT License.
```

This README should cover the core aspects of your application: features, how it works, usage instructions, running tests, and logging details. Let me know if you need more details!
