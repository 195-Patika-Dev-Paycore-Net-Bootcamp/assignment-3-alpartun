using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using payCoreHW3.Context;
using payCoreHW3.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace payCoreHW3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContainerController : ControllerBase
    {
        private readonly IMapperSession _session;

        public ContainerController(IMapperSession session)
        {
            _session = session;
        }

        [HttpGet]
        public List<Container> Get()
        {
            var result = _session.Containers.ToList();
            return result;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Container container)
        {
            try
            {
                _session.BeginTransaction();
                _session.Save(container);
                _session.Commit();
            }
            catch (Exception e)
            {
                _session.Rollback();
                return BadRequest("Container creation error.");
            }
            finally
            {
                _session.CloseTransaction();
            }

            return Ok(container);
        }

        [HttpPut]
        public IActionResult Put(Container container)
        {
            if (container == null) return BadRequest("Container is null.");

            var oldContainer = _session.Containers.Where(x => x.Id == container.Id).FirstOrDefault();
            if (oldContainer == null) return BadRequest("There is no such kind of container in datavase.");
            Container newContainer = new()
            {
                Id = oldContainer.VehicleId,
                ContainerName = container.ContainerName,
                Latitude = container.Latitude,
                Longitude = container.Longitude,
                VehicleId = oldContainer.VehicleId
            };


            try
            {
                _session.BeginTransaction();
                _session.Update(newContainer);
                _session.Commit();
            }

            catch (Exception ex)
            {
                _session.Rollback();
                return BadRequest("Exception");
            }
            finally
            {
                _session.CloseTransaction();
            }

            return Ok(new { message = "Container updated." });
        }

        [HttpDelete]
        public IActionResult Delete(long id)
        {
            var containerDelete = _session.Containers.Where(x => x.Id == id).FirstOrDefault();

            if (containerDelete == null) return BadRequest("Container does not exists.");
            try
            {
                _session.BeginTransaction();
                _session.Delete(containerDelete);
                _session.Commit();
            }
            catch (Exception e)
            {
                _session.Rollback();
                return BadRequest("Error has occured.");
            }
            finally
            {
                _session.CloseTransaction();
            }

            return Ok("Container deleted successfully.");
        }

        [HttpGet("{vehicleId}")]
        public IActionResult GetContainersByVehicleId(long vehicleId)
        {
            var containers = _session.Containers.Where(x => x.VehicleId == vehicleId).ToList();

            if (containers.Count == 0) return Ok("Does not exists.");

            return Ok(containers);
        }

        [HttpPost("Cluster")]
        public IActionResult Cluster([FromQuery] long vehicleId, int numberOfClusters)
        {
            var containerList = _session.Containers.Where(x => x.VehicleId == vehicleId).ToList();
            //int divide = containerList.Count / numberOfClusters;
            //Gelen n degerini kontrol et 0 olamaz, container sayisindan buyuk olamaz.
            var list = containerList.Select(
                    (container, index) => new
                    {
                        ClusterIndex = index % numberOfClusters, Item = container
                    })
                .GroupBy(i => i.ClusterIndex, g => g.Item).ToList();
            return Ok(list);
        }
    }
}