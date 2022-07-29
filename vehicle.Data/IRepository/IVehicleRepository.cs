using Newtonsoft.Json.Linq;
using System.Data;
using vehicle.Data.Models;

namespace vehicle.Data.IRepository
{
    public interface IVehicleRepository
    {
        JArray GetVehicle(int Id);
        DataSet AddUpdateVehicle(Vehicle vehicle);
        DataSet Delete(int Id);
    }
}
