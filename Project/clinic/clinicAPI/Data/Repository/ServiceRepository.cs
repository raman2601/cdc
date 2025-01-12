using clinicAPI.Data.Interfaces;
using clinicAPI.Data.Model;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace clinicAPI.Data.Repository
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly string _connectionString;

        public ServiceRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Service> CreateServiceAsync(Service service)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("INSERT INTO Services (ServiceName, Description, Cost, CreatedAt, UpdatedAt) " +
                                              "OUTPUT INSERTED.ServiceID VALUES (@ServiceName, @Description, @Cost, GETDATE(), GETDATE());", connection);

                command.Parameters.Add(new SqlParameter("@ServiceName", service.ServiceName));
                command.Parameters.Add(new SqlParameter("@Description", (object)service.Description ?? DBNull.Value)); // Handle nulls
                command.Parameters.Add(new SqlParameter("@Cost", service.Cost));

                service.ServiceID = (int)await command.ExecuteScalarAsync();
                return service;
            }
        }

        public async Task<Service> GetServiceByIdAsync(int serviceId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Services WHERE ServiceID = @ServiceID", connection);
                command.Parameters.Add(new SqlParameter("@ServiceID", serviceId));

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Service
                        {
                            ServiceID = (int)reader["ServiceID"],
                            ServiceName = (string)reader["ServiceName"],
                            Description = reader["Description"] as string,
                            Cost = (decimal)reader["Cost"],
                            CreatedAt = (DateTime)reader["CreatedAt"],
                            UpdatedAt = (DateTime)reader["UpdatedAt"]
                        };
                    }
                }
            }
            return null; // Return null if not found
        }

        public async Task<IEnumerable<Service>> GetAllServicesAsync()
        {
            var services = new List<Service>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Services", connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        services.Add(new Service
                        {
                            ServiceID = (int)reader["ServiceID"],
                            ServiceName = (string)reader["ServiceName"],
                            Description = reader["Description"] as string,
                            Cost = (decimal)reader["Cost"],
                            CreatedAt = (DateTime)reader["CreatedAt"],
                            UpdatedAt = (DateTime)reader["UpdatedAt"]
                        });
                    }
                }
            }

            return services; // Return the list of services
        }

        public async Task<bool> UpdateServiceAsync(Service service)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("UPDATE Services SET ServiceName = @ServiceName, Description = @Description, " +
                                              "Cost = @Cost, UpdatedAt = GETDATE() WHERE ServiceID = @ServiceID", connection);

                command.Parameters.Add(new SqlParameter("@ServiceID", service.ServiceID));
                command.Parameters.Add(new SqlParameter("@ServiceName", service.ServiceName));
                command.Parameters.Add(new SqlParameter("@Description", (object)service.Description ?? DBNull.Value)); // Handle nulls
                command.Parameters.Add(new SqlParameter("@Cost", service.Cost));

                var affectedRows = await command.ExecuteNonQueryAsync();
                return affectedRows > 0; // Return true if the update was successful
            }
        }

        public async Task<bool> DeleteServiceAsync(int serviceId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("DELETE FROM Services WHERE ServiceID = @ServiceID", connection);
                command.Parameters.Add(new SqlParameter("@ServiceID", serviceId));

                var affectedRows = await command.ExecuteNonQueryAsync();
                return affectedRows > 0; // Return true if the deletion was successful
            }
        }
    }
}
