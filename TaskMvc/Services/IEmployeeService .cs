using TaskMvc.Models;

namespace TaskMvc.Services
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<(int Total, int Saved, int Skipped, List<string> SkippedPayrolls)> ImportEmployeesAsync(IFormFile file);
        Task<Employee?> GetEmployeeByIdAsync(string payrollNumber);
        Task<bool> UpdateEmployeeAsync(Employee employee);
    }
}
