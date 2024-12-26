namespace RentalCar.Model.Core.Services;

public interface IPrometheusService
{
    void AddNewModelCounter(string statusCodes);
    void AddDeleteModelCounter(string statusCodes);
    void AddUpdateModelCounter(string statusCodes);
    void AddUpdateStatusModelCounter(string statusCodes);
    void AddFindByIdModelCounter(string statusCodes);
    void AddFindAllModelsCounter(string statusCodes);
}