using Microsoft.AspNetCore.Mvc;
using payCoreHW3.Context;
using payCoreHW3.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace payCoreHW3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IMapperSession _session;

        public VehicleController(IMapperSession session)
        {
            _session = session;
        }        
        
        [HttpGet]
        public List<Vehicle> Get()
        {
            var result = _session.Vehicles.ToList();
            return result;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            var result = _session.Vehicles.Where(x => x.Id == id).FirstOrDefault();
            if (result == null) return BadRequest("Vehicle id is wrong.");
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Vehicle vehicle)
        {
            try
            {
                _session.BeginTransaction();
                _session.Save(vehicle);
                _session.Commit();

            }
            catch (Exception exception)
            {
                _session.Rollback();
                return BadRequest("Creation error.");
            }
            finally
            {
                _session.CloseTransaction();
            }

            return Ok("Successfully created.");
        }

        [HttpPut]
        public ActionResult<Vehicle> Put([FromBody] Vehicle vehicleRequest)
        {
            var vehicle = _session.Vehicles.Where(x => x.Id == vehicleRequest.Id).FirstOrDefault();
            if (vehicle == null) return NotFound("Arac bulunamadi.");

            try
            {
                _session.BeginTransaction();

                vehicle.VehicleName = vehicleRequest.VehicleName;
                vehicle.VehiclePlate = vehicleRequest.VehiclePlate;
                _session.Update(vehicle);
                _session.Commit();

            }
            catch (Exception e)
            {

                _session.Rollback();
                return BadRequest("Put process error.");
            }
            finally
            {
                _session.CloseTransaction();
            }

            return Ok(vehicle);
            
        }

        [HttpDelete("{id}")]

        public IActionResult Delete(long id)
        {
            var vehicle = _session.Vehicles.Where(x => x.Id == id).FirstOrDefault();
            var vehiclesContainer = _session.Containers.Where(x => x.VehicleId == id).ToList();
            if (vehicle == null) return BadRequest("Vehicle can not found.");

            try
            {
                _session.BeginTransaction();
                if (vehiclesContainer.Count!=0)
                {
                    foreach (var container in vehiclesContainer)
                    {
                        _session.Delete(container);
                        
                    }

                }
                _session.Delete(vehicle);

                _session.Commit();

            }
            catch (Exception e)
            {
                _session.Rollback();
                throw;
            }
            finally
            {
                _session.CloseTransaction();
            }

            return Ok("Delete operation success");
        }
    }
}

