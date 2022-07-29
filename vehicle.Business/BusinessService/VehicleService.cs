using Common;
using Newtonsoft.Json.Linq;
using vehicle.Business.IBusinessService;
using vehicle.Data.IRepository;
using vehicle.Data.Models;

namespace vehicle.Business.BusinessService
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            this.vehicleRepository = vehicleRepository;
        }


        public JArray GetVehicle(int Id)
        {
            return vehicleRepository.GetVehicle(Id);
        }

        public JArray AddUpdateVehicle(Vehicle vehicle)
        {
            var data = vehicleRepository.AddUpdateVehicle(vehicle);

            return data.Tables[0].ToJArray();
        }

        public JArray DeleteVehicle(int Id)
        {
            var data = vehicleRepository.Delete(Id);

            return data.Tables[0].ToJArray();
        }
    }
}
