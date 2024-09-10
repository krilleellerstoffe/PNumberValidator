
namespace PNumberValidatorTests;

using PNumberValidator;


[TestClass]
public class PersonnummerTests
{
    
    [TestMethod]
    public void TestInvalidLength()
    {
        // Arrange
        string[] invalidLengths = {
            "20170110", // too short
            "20170110238456", // too long
            "12345", // too short
            "12345678901234567890" // too long
        };

        // Act & Assert
        foreach (var number in invalidLengths)
        {
            var result = PersonnummerValidator.ValidateNumber(number);
            Assert.IsFalse(result, $"{number} should be invalid due to incorrect length.");
        }
    }

    [TestMethod]
    public void TestWrongCharacters()
    {
        // Arrange
        string[] invalidCharacters = {
            "20170110ABCD", // contains letters
            "20170110-12A4", // contains letters
            "20170110 1234", // contains space
            "20170110!1234" // contains special character
        };

        // Act & Assert
        foreach (var number in invalidCharacters)
        {
            var result = PersonnummerValidator.ValidateNumber(number);
            Assert.IsFalse(result, $"{number} should be invalid due to wrong characters.");
        }
    }

    [TestMethod]
    public void TestInvalidDate()
    {
        // Arrange
        string[] invalidDates = {
            "20191301-1234", // Invalid month
            "20190230-1234", // Invalid day
            "19000230-1234", // Invalid day in leap year
            "20170132-1234"  // Invalid day
        };

        // Act & Assert
        foreach (var number in invalidDates)
        {
            var result = PersonnummerValidator.ValidateNumber(number);
            Assert.IsFalse(result, $"{number} should be invalid due to incorrect date.");
        }
    }

    [TestMethod]
    public void TestSpecialCharacters()
    {
        // Arrange
        string[] specialCharacters = {
            "20170110*1234", // contains *
            "20170110/1234", // contains /
            "20170110@1234", // contains @
            "20170110#1234"  // contains #
        };

        // Act & Assert
        foreach (var number in specialCharacters)
        {
            var result = PersonnummerValidator.ValidateNumber(number);
            Assert.IsFalse(result, $"{number} should be invalid due to special characters.");
        }
    }

    [TestMethod]
    public void TestEmptyInput()
    {
        // Arrange
        string[] emptyInputs = {
            "", // empty string
            " ", // space
            null // null
        };

        // Act & Assert
        foreach (var number in emptyInputs)
        {
            var result = PersonnummerValidator.ValidateNumber(number);
            Assert.IsFalse(result, $"'{number}' should be invalid as it's empty or null.");
        }
    }

    [TestMethod]
    public void TestValidPersonnummer()
    {
        // Arrange
        string[] validNumbers = { //list provided from skatteverket
            "201701102384",
            "141206-2380",
            "20080903-2386",
            "7101169295",
            "198107249289",
            "19021214-9819",
            "190910199827",
            "191006089807",
            "192109099180",
            "4607137454",
            "194510168885",
            "900118+9811",
            "189102279800",
            "189912299816"
        };

        // Act & Assert
        foreach (var number in validNumbers)
        {
            var result = PersonnummerValidator.ValidateNumber(number);
            Assert.IsTrue(result, $"{number} should be a valid personnummer.");
        }
    }

    [TestMethod]
    public void TestInvalidPersonnummer()
    {
        // Arrange
        string[] invalidNumbers = { // list provided from skatteverket
            "201701272394",
            "190302299813"
        };

        // Act & Assert
        foreach (var number in invalidNumbers)
        {
            var result = PersonnummerValidator.ValidateNumber(number);
            Assert.IsFalse(result, $"{number} should be invalid.");
        }
    }

    [TestMethod]
    public void TestValidSamordningsnummer()
    {
        // Arrange
        string[] validNumbers = { // list provided from skatteverket
            "190910799824"
        };

        // Act & Assert
        foreach (var number in validNumbers)
        {
            var result = PersonnummerValidator.ValidateNumber(number);
            Assert.IsTrue(result, $"{number} should be a valid samordningsnummer.");
        }
    }

    [TestMethod]
    public void TestValidOrganisationsnummer()
    {
        // Arrange
        string[] validNumbers = { // list provided from skatteverket
            "556614-3185",
            "16556601-6399",
            "262000-1111",
            "857202-7566"
        };

        // Act & Assert
        foreach (var number in validNumbers)
        {
            var result = PersonnummerValidator.ValidateNumber(number);
            Assert.IsTrue(result, $"{number} should be a valid Organisationsnummer.");
        }
    }
}