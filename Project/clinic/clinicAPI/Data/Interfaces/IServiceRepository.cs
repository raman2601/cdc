using clinicAPI.Data.Model;

namespace clinicAPI.Data.Interfaces
{
    public interface IServiceRepository
    {
        Task<Service> CreateServiceAsync(Service service);
        Task<Service> GetServiceByIdAsync(int serviceId);
        Task<IEnumerable<Service>> GetAllServicesAsync();
        Task<bool> UpdateServiceAsync(Service service);
        Task<bool> DeleteServiceAsync(int serviceId);
    }
}
