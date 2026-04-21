using static ProductsAPI.Enums.Status;

namespace ProductsAPI.Models
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string? Message { get; set; }

        public ServiceStatus Status { get; set; }
    }
}
