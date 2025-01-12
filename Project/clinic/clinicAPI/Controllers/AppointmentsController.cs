using clinicAPI.Data.Interfaces;
using clinicAPI.Data.Model;
using Microsoft.AspNetCore.Mvc;

namespace clinicAPI.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentsController(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        [HttpPost("book")]
        public async Task<IActionResult> BookAppointment([FromBody] Appointment appointment)
        {
            if (appointment == null || appointment.PatientID <= 0 || appointment.DoctorID <= 0)
            {
                return BadRequest("Invalid appointment data.");
            }

            var createdAppointment = await _appointmentRepository.BookAppointmentAsync(appointment);
            return CreatedAtAction(nameof(GetAppointmentById), new { id = createdAppointment.AppointmentID }, createdAppointment);
        }

        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            var result = await _appointmentRepository.CancelAppointmentAsync(id);
            if (!result)
            {
                return NotFound("Appointment not found.");
            }
            return NoContent();
        }

        [HttpPut("reschedule/{id}")]
        public async Task<IActionResult> RescheduleAppointment(int id, [FromBody] Appointment appointment)
        {
            if (appointment == null || appointment.AppointmentDate == default || appointment.AppointmentTime == default)
            {
                return BadRequest("Invalid rescheduling data.");
            }

            var result = await _appointmentRepository.RescheduleAppointmentAsync(id, appointment.AppointmentDate, appointment.AppointmentTime);
            if (!result)
            {
                return NotFound("Appointment not found.");
            }
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(id);
            if (appointment == null)
            {
                return NotFound("Appointment not found.");
            }
            return Ok(appointment);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            var appointments = await _appointmentRepository.GetAllAppointmentsAsync();
            return Ok(appointments);
        }
    }

}
