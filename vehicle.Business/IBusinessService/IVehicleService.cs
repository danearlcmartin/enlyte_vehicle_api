using Newtonsoft.Json.Linq;
using vehicle.Data.Models;

namespace vehicle.Business.IBusinessService
{
    public interface IVehicleService
    {
        JArray GetVehicle(int Id);
        JArray AddUpdateVehicle(Vehicle vehicle);
        JArray DeleteVehicle(int Id);

    }
}
