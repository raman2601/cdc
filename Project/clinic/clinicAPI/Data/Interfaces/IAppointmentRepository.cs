using clinicAPI.Data.Model;

namespace clinicAPI.Data.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<Appointment> BookAppointmentAsync(Appointment appointment);
        Task<bool> CancelAppointmentAsync(int appointmentId);
        Task<bool> RescheduleAppointmentAsync(int appointmentId, DateTime newDate, TimeSpan newTime);
        Task<Appointment> GetAppointmentByIdAsync(int appointmentId);
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
    }

}
