using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TaskMvc.Data;
using TaskMvc.Models;
using TaskMvc.Services;
using Xunit;

namespace TaskMvc.Tests
{
    public class EmployeeServiceTests
    {
        private async Task<AppDbContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "EmployeeDb_Test")
                .Options;

            var dbContext = new AppDbContext(options);
            await dbContext.Database.EnsureCreatedAsync();

            return dbContext;
        }

        [Fact]
        public async Task ImportEmployees_ShouldAddNewEmployees_AndSkipExisting_OnPayrollNumber()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var service = new EmployeeService(dbContext);

            // Add existing employee
            var existingEmployee = new Employee
            {
                PayrollNumber = "123",
                Forenames = "John",
                Surname = "Doe",
                DateOfBirth = DateTime.Parse("2025-03-06"),
                Telephone = "123456",
                Mobile = "789456",
                Address = "123 Street",
                Address2 = "Apt 4B",
                Postcode = "12345",
                EmailHome = "john@example.com",
                StartDate = DateTime.Parse("2025-03-06")
            };

            dbContext.Employees.Add(existingEmployee);
            await dbContext.SaveChangesAsync();

            // Mock CSV Data (One duplicate and one new)
            var csvData = new StringBuilder();
            csvData.AppendLine("Personnel_Records.Payroll_Number,Personnel_Records.Forenames,Personnel_Records.Surname,Personnel_Records.Date_of_Birth,Personnel_Records.Telephone,Personnel_Records.Mobile,Personnel_Records.Address,Personnel_Records.Address_2,Personnel_Records.Postcode,Personnel_Records.EMail_Home,Personnel_Records.Start_Date");
            csvData.AppendLine("123,John,Doe,1990-01-01,123456,789456,123 Street,Apt 4B,12345,john@example.com,2023-01-01"); // Duplicate
            csvData.AppendLine("456,Jane,Smith,1992-02-02,654321,987654,456 Avenue,Apt 2A,54321,jane@example.com,2023-02-02"); // New

            // Convert CSV data to a byte array
            var csvBytes = Encoding.UTF8.GetBytes(csvData.ToString());

            // Create a mock IFormFile
            var formFile = new FormFile(new MemoryStream(csvBytes), 0, csvBytes.Length, "file", "employees.csv")
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/csv"
            };

            // Act
            var (total, saved, skipped, skippedPayrolls) = await service.ImportEmployeesAsync(formFile);

            // Assert
            total.Should().Be(2);    // Two records in CSV
            saved.Should().Be(1);    // One new employee should be added
            skipped.Should().Be(1);  // One duplicate should be skipped
            skippedPayrolls.Should().Contain("123");

            var dbEmployees = dbContext.Employees.ToList();
            dbEmployees.Should().HaveCount(2); // Should contain both employees
        }
    }
}
