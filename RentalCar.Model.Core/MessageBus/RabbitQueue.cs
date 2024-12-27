namespace RentalCar.Model.Core.MessageBus;

public class RabbitQueue
{
    //CATEGORY
    public static string CATEGORY_MODEL_NEW_REQUEST_QUEUE = "CategoryModelNewRequestQueue";
    public static string? CATEGORY_MODEL_NEW_RESPONSE_QUEUE = "CategoryModelNewResponseQueue";
    
    public static string CATEGORY_MODEL_FIND_REQUEST_QUEUE = "CategoryModelFindRequestQueue";
    public static string CATEGORY_MODEL_FIND_RESPONSE_QUEUE = "CategoryModelFindResponseQueue";
    
    public static string CATEGORY_MODEL_UPDATE_REQUEST_QUEUE = "CategoryModelUpdateRequestQueue";
    public static string CATEGORY_MODEL_UPDATE_RESPONSE_QUEUE = "CategoryModelUpdateResponseQueue";
    
    // MANUFACTURER
    public static string MANUFACTURER_MODEL_NEW_REQUEST_QUEUE = "ManufacturerModelNewRequestQueue";
    public static string MANUFACTURER_MODEL_NEW_RESPONSE_QUEUE = "ManufacturerModelNewResponseQueue";
    
    public static string MANUFACTURER_MODEL_FIND_REQUEST_QUEUE = "ManufacturerModelFindRequestQueue";
    public static string MANUFACTURER_MODEL_FIND_RESPONSE_QUEUE = "ManufacturerModelFindResponseQueue";
    
    public static string MANUFACTURER_MODEL_UPDATE_REQUEST_QUEUE = "ManufacturerModelUpdateRequestQueue";
    public static string MANUFACTURER_MODEL_UPDATE_RESPONSE_QUEUE = "ManufacturerModelUpdateResponseQueue";
}