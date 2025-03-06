using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using TaskMvc.Data;
using TaskMvc.Models;

namespace TaskMvc.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _context;

        public EmployeeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees.OrderBy(e => e.Surname).ToListAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(string payrollNumber)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.PayrollNumber == payrollNumber);
        }

        public async Task<(int Total, int Saved, int Skipped, List<string> SkippedPayrolls)> ImportEmployeesAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) throw new ArgumentException("File is invalid or empty.");

            int totalRecords = 0, savedRecords = 0, skippedRecords = 0;
            List<string> skippedPayrollNumbers = new List<string>();

            using (var stream = file.OpenReadStream())
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                var lines = new List<string>();
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (line.StartsWith('"') && line.EndsWith('"'))
                        line = line.Substring(1, line.Length - 2);
                    lines.Add(line);
                }

                using (var stringReader = new StringReader(string.Join("\n", lines)))
                using (var csv = new CsvReader(stringReader, new CsvConfiguration(CultureInfo.GetCultureInfo("en-GB"))))
                {
                    csv.Context.RegisterClassMap<EmployeeMap>();
                    var records = csv.GetRecords<Employee>().ToList();
                    totalRecords = records.Count;

                    foreach (var employee in records)
                    {
                        if (await _context.Employees.AnyAsync(e => e.PayrollNumber == employee.PayrollNumber))
                        {
                            skippedRecords++;
                            skippedPayrollNumbers.Add(employee.PayrollNumber);
                        }
                        else
                        {
                            _context.Employees.Add(employee);
                            savedRecords++;
                        }
                    }

                    await _context.SaveChangesAsync();
                }
            }

            return (totalRecords, savedRecords, skippedRecords, skippedPayrollNumbers);
        }

        public async Task<bool> UpdateEmployeeAsync(Employee employee)
        {
            var existingEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.PayrollNumber == employee.PayrollNumber);
            if (existingEmployee == null) return false;

            existingEmployee.Forenames = employee.Forenames;
            existingEmployee.Surname = employee.Surname;
            existingEmployee.DateOfBirth = employee.DateOfBirth;
            existingEmployee.Telephone = employee.Telephone;
            existingEmployee.Mobile = employee.Mobile;
            existingEmployee.Address = employee.Address;
            existingEmployee.EmailHome = employee.EmailHome;
            existingEmployee.StartDate = employee.StartDate;

            await _context.SaveChangesAsync();
            return true;
        }

    }
}
