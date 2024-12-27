using RentalCar.Model.Core.Enuns;

namespace RentalCar.Model.Application.Validators;

public class CustomValidator
{
    public static bool ValidTransmission(char value)
    {
        var itens = Enum.GetValues(typeof(Transmission)).Cast<char>().ToList();
        return itens.Any(item => item == value);
    }

    public static bool ValidMotor(char value)
    {
        var itens = Enum.GetValues(typeof(Motor)).Cast<char>().ToList();
        return itens.Any(item => item == value);
    }
}