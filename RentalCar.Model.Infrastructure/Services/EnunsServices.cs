using RentalCar.Model.Core.Enuns;

namespace RentalCar.Model.Infrastructure.Services;

public class EnunsServices
{
    public static Motor GetMotor(char value) => value switch
    {
        //var item = Enum.GetValues(typeof(Motor)).Cast<Motor>().FirstOrDefault(item => item.Equals(value));
        //return item;
        'H' => Motor.Hibrid,
        'D' => Motor.Diesel,
        'E' => Motor.Electric,
        'G' => Motor.Gasoline
    };

    public static Transmission GetTransmission(char value) => value switch
    {
        'A' => Transmission.Automatic,
        'M' => Transmission.Manual
    };

    public static string GetDescriptionMotor(Motor motor) => motor switch
    {
        Motor.Electric => "Electrico",
        Motor.Diesel => "Diesel",
        Motor.Gasoline => "Gasolina",
        Motor.Hibrid => "Hibrido",
        _ => "NA"
    };

    public static string GetDescriptionTransmission(Transmission transmission) =>  transmission switch
    {
        Transmission.Manual => "Manual",
        Transmission.Automatic => "Automático",
        _ => "NA"
    };
    
    public static string GetDescriptionStatus(Status status) =>  status switch
    {
        Status.Created => "Criado",
        Status.Deleted => "Eliminado",
        Status.Pending => "Pendente",
        Status.Suspended => "Suspendido",
        _ => "NA"
    };
    
}