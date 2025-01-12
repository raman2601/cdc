using clinicAPI.Data.Model;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;


namespace clinicAPI.Data.Interfaces
{

    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly string _connectionString;

        public AppointmentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Appointment> BookAppointmentAsync(Appointment appointment)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, CreatedAt, UpdatedAt) " +
                                              "OUTPUT INSERTED.AppointmentID VALUES (@PatientID, @DoctorID, @AppointmentDate, @AppointmentTime, @Status, GETDATE(), GETDATE());", connection);

                command.Parameters.Add(new SqlParameter("@PatientID", appointment.PatientID));
                command.Parameters.Add(new SqlParameter("@DoctorID", appointment.DoctorID));
                command.Parameters.Add(new SqlParameter("@AppointmentDate", appointment.AppointmentDate));
                command.Parameters.Add(new SqlParameter("@AppointmentTime", appointment.AppointmentTime));
                command.Parameters.Add(new SqlParameter("@Status", appointment.Status));

                appointment.AppointmentID = (int)await command.ExecuteScalarAsync();
                return appointment;
            }
        }

        public async Task<bool> CancelAppointmentAsync(int appointmentId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("UPDATE Appointments SET Status = 'Cancelled', UpdatedAt = GETDATE() WHERE AppointmentID = @AppointmentID", connection);
                command.Parameters.Add(new SqlParameter("@AppointmentID", appointmentId));

                var affectedRows = await command.ExecuteNonQueryAsync();
                return affectedRows > 0; // Return true if the cancellation was successful
            }
        }

        public async Task<bool> RescheduleAppointmentAsync(int appointmentId, DateTime newDate, TimeSpan newTime)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("UPDATE Appointments SET AppointmentDate = @NewDate, AppointmentTime = @NewTime, Status = 'Rescheduled', UpdatedAt = GETDATE() WHERE AppointmentID = @AppointmentID", connection);

                command.Parameters.Add(new SqlParameter("@AppointmentID", appointmentId));
                command.Parameters.Add(new SqlParameter("@NewDate", newDate));
                command.Parameters.Add(new SqlParameter("@NewTime", newTime));

                var affectedRows = await command.ExecuteNonQueryAsync();
                return affectedRows > 0; // Return true if the rescheduling was successful
            }
        }

        public async Task<Appointment> GetAppointmentByIdAsync(int appointmentId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Appointments WHERE AppointmentID = @AppointmentID", connection);
                command.Parameters.Add(new SqlParameter("@AppointmentID", appointmentId));

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Appointment
                        {
                            AppointmentID = (int)reader["AppointmentID"],
                            PatientID = (int)reader["PatientID"],
                            DoctorID = (int)reader["DoctorID"],
                            AppointmentDate = (DateTime)reader["AppointmentDate"],
                            AppointmentTime = (TimeSpan)reader["AppointmentTime"],
                            Status = (string)reader["Status"],
                            CreatedAt = (DateTime)reader["CreatedAt"],
                            UpdatedAt = (DateTime)reader["UpdatedAt"]
                        };
                    }
                }
            }
            return null; // Return null if not found
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            var appointments = new List<Appointment>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Appointments", connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        appointments.Add(new Appointment
                        {
                            AppointmentID = (int)reader["AppointmentID"],
                            PatientID = (int)reader["PatientID"],
                            DoctorID = (int)reader["DoctorID"],
                            AppointmentDate = (DateTime)reader["AppointmentDate"],
                            AppointmentTime = (TimeSpan)reader["AppointmentTime"],
                            Status = (string)reader["Status"],
                            CreatedAt = (DateTime)reader["CreatedAt"],
                            UpdatedAt = (DateTime)reader["UpdatedAt"]
                        });
                    }
                }
            }

            return appointments; // Return the list of appointments
        }
    }

}
