using clinicAPI.Data.Interfaces;
using clinicAPI.Data.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace clinicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepository;

        public ServicesController(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateService([FromBody] Service service)
        {
            if (service == null || string.IsNullOrEmpty(service.ServiceName))
            {
                return BadRequest("Invalid service data.");
            }

            var createdService = await _serviceRepository.CreateServiceAsync(service);
            return CreatedAtAction(nameof(GetServiceById), new { id = createdService.ServiceID }, createdService);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceById(int id)
        {
            var service = await _serviceRepository.GetServiceByIdAsync(id);
            if (service == null)
            {
                return NotFound("Service not found.");
            }
            return Ok(service);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllServices()
        {
            var services = await _serviceRepository.GetAllServicesAsync();
            return Ok(services);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(int id, [FromBody] Service service)
        {
            if (service == null || id != service.ServiceID)
            {
                return BadRequest("Invalid service data.");
            }

            var result = await _serviceRepository.UpdateServiceAsync(service);
            if (!result)
            {
                return NotFound("Service not found.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var result = await _serviceRepository.DeleteServiceAsync(id);
            if (!result)
            {
                return NotFound("Service not found.");
            }
            return NoContent();
        }
    }
}
