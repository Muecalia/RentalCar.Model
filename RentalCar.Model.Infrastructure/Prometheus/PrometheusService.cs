using RentalCar.Model.Core.Services;

namespace RentalCar.Model.Infrastructure.Prometheus;

public class PrometheusService : IPrometheusService
{
    //private static readonly Counter RequestAccountCounter = Metrics.CreateCounter("account_total", "Total requisições de criação de conta", ["status_code"]);
    //private static readonly Counter RequestRoleCounter = Metrics.CreateCounter("role_total", "Total requisições de criação de perfil", ["status_code"]);
    //private static readonly Counter RequestLoginCounter = Metrics.CreateCounter("login_total", "Total requisições de login (acesso dos utilizadores)", ["status_code"]);
    //private static readonly Counter RequestLoginCounter = Metrics.CreateCounter("login_total", "Total requisições de login (acesso dos utilizadores)", ["status_code"]);
    
    

    public void AddNewModelCounter(string statusCodes)
    {
        System.Diagnostics.Debug.Print(statusCodes);
    }

    public void AddDeleteModelCounter(string statusCodes)
    {
        System.Diagnostics.Debug.Print(statusCodes);
    }

    public void AddUpdateModelCounter(string statusCodes)
    {
        System.Diagnostics.Debug.Print(statusCodes);
    }

    public void AddUpdateStatusModelCounter(string statusCodes)
    {
        System.Diagnostics.Debug.Print(statusCodes);
    }

    public void AddFindByIdModelCounter(string statusCodes)
    {
        System.Diagnostics.Debug.Print(statusCodes);
    }

    public void AddFindAllModelsCounter(string statusCodes)
    {
        System.Diagnostics.Debug.Print(statusCodes);
    }
    
}