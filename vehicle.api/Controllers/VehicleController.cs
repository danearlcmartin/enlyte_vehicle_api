using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using vehicle.Business.IBusinessService;
using vehicle.Data.Models;

namespace vehicle.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private IVehicleService _service = null;

        public VehicleController(IVehicleService service)
        {
            this._service = service;
        }

        [HttpGet("GetVehicle/{id}")]
        public IActionResult GetVehicle([FromRoute] int Id)
        {
            var data = _service.GetVehicle(Id);

            return Ok(data);
        }

        [HttpPost]
        public IActionResult AddVehicle([FromBody] Vehicle vehicle)
        {
            var data = _service.AddUpdateVehicle(vehicle);

            return Ok(data);
        }

        [HttpPut]
        public IActionResult UpdateVehicle([FromBody] Vehicle vehicle)
        {
            var data = _service.AddUpdateVehicle(vehicle);

            return Ok(data);
        }

        [HttpDelete]
        public IActionResult DeleteVehicle(int Id)
        {
            var data = _service.DeleteVehicle(Id);

            return Ok(data);
        }
    }
}
