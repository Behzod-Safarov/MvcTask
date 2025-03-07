# Employee Management System

## Overview
This is a simple **ASP.NET MVC** web application that allows users to import employee data from a CSV file into a **SQL Server** database. The application displays the imported data in a grid with sorting, searching, and editing functionalities.

## Features
- **CSV File Import**: Uploads and parses a CSV file, inserting employee data into the database.
- **Data Grid Display**: Displays imported employees in a sortable and searchable table.
- **Editing Functionality**: Users can edit employee records using a modal popup.
- **Real-time Updates**: The table updates automatically after importing or editing data.
- **Third-Party Grid**: Uses **DataTables.js** for enhanced table functionalities.

## Technologies Used
- **ASP.NET MVC 5**
- **Entity Framework Core**
- **SQL Server**
- **jQuery & AJAX**
- **DataTables.js** (for table enhancements)
- **Bootstrap 4** (for UI styling)

## Installation
### Prerequisites
- .NET Framework (or .NET Core for modern implementation)
- SQL Server
- Visual Studio

### Steps to Set Up the Project
1. **Clone the repository**:
   ```sh
   git clone https://github.com/Behzod-Safarov/MvcTask.git
   cd MvcTask
   ```
2. **Set up the database**:
   - Open **SQL Server Management Studio (SSMS)**.
   - Create a new database: `EmployeeDB`.
   - Update `appsettings.json` (or `Web.config`) with your database connection string.
   - Run the **Entity Framework migrations** to generate the `Employees` table:
     ```sh
     Update-Database
     ```
3. **Run the project**:
   - Open the project in **Visual Studio**.
   - Build the solution and run the project (`Ctrl + F5`).

## Project Structure
```
MvcTask/
│── Controllers/
│   ├── HomeController.cs  # Handles file upload & grid operations
│   ├── EmployeeController.cs  # Handles editing and updating employees
│
│── Models/
│   ├── Employee.cs  # Defines the Employee model
│
│── Services/
│   ├── EmployeeService.cs  # Handles database operations
│
│── Views/
│   ├── Home/
│   │   ├── Index.cshtml  # Main page with file upload and grid
│   │   ├── _EditEmployeeModal.cshtml  # Partial view for editing
│
│── wwwroot/
│   ├── js/
│   │   ├── site.js  # Custom JavaScript for handling AJAX operations
│
│── README.md  # Project documentation
```

## Usage
1. **Upload CSV**:
   - Click **Browse**, select a CSV file, and click **Import**.
   - The program will process the file and display a success message.
2. **View & Search Data**:
   - Imported employee records will appear in the table.
   - Use the **search bar** or click column headers to sort data.
3. **Edit Employees**:
   - Click **Edit** next to an employee record.
   - Modify details in the popup modal and click **Save Changes**.
   - The grid updates in real-time.

## Unit Testing
Unit tests are written using **xUnit/NUnit**.
To run the tests:
```sh
cd MvcTask.Tests
 dotnet test
```

## Future Enhancements
- Implementing **CSV validation** before import.
- Adding **bulk edit** functionality.
- Improving UI with a modern frontend framework (e.g., React or Blazor).

---

### Author
Developed by **Bekhzod Safarov**. 🚀

