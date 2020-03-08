using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeeList;
        public MockEmployeeRepository()
        {
            _employeeList = new List<Employee>()
            {
                new Employee(){ Id = 1, Name = "Ashish", Email = "abcd@bcde.com", Department = "IT"},
                new Employee(){ Id = 2, Name = "Pradnya", Email = "abcde@bcde.com", Department = "HR"},
                new Employee(){ Id = 3, Name = "Monu", Email = "abcdef@bcde.com", Department = "IT"}
            };
        }
        public Employee GetEmployee(int Id)
        {
            return _employeeList.FirstOrDefault(e => e.Id == Id);
        }
    }
}
