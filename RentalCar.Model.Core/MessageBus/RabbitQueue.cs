namespace RentalCar.Model.Core.MessageBus;

public class RabbitQueue
{
    //CATEGORY
    public static string CATEGORY_MODEL_REQUEST_QUEUE = "CategoryModelRequestQueue";
    public static string CATEGORY_MODEL_RESPONSE_QUEUE = "CategoryModelResponseQueue";
    
    public static string FIND_CATEGORY_MODEL_REQUEST_QUEUE = "FindCategoryModelRequestQueue";
    public static string FIND_CATEGORY_MODEL_RESPONSE_QUEUE = "FindCategoryModelResponseQueue";
    
    // MANUFACTURER
    public static string MANUFACTURER_MODEL_REQUEST_QUEUE = "ManufacturerModelRequestQueue";
    public static string MANUFACTURER_MODEL_RESPONSE_QUEUE = "ManufacturerModelResponseQueue";
    
    public static string FIND_MANUFACTURER_MODEL_REQUEST_QUEUE = "FindManufacturerModelRequestQueue";
    public static string FIND_MANUFACTURER_MODEL_RESPONSE_QUEUE = "FindManufacturerModelResponseQueue";
    
}