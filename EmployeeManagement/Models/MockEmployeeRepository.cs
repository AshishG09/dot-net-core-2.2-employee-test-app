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
                new Employee(){ Id = 1, Name = "Ashish", Email = "abcd@bcde.com", Department = DeptEnum.IT},
                new Employee(){ Id = 2, Name = "Pradnya", Email = "abcde@bcde.com", Department = DeptEnum.HR},
                new Employee(){ Id = 3, Name = "Monu", Email = "abcdef@bcde.com", Department = DeptEnum.IT}
            };
        }

        public Employee Add(Employee employee)
        {
            employee.Id = _employeeList.Max(e => e.Id) + 1;
            _employeeList.Add(employee);
            return employee;
        }

        public Employee GetEmployee(int Id)
        {
            return _employeeList.FirstOrDefault(e => e.Id == Id);
        }

        public IEnumerable<Employee> GetEmployees()
        {
            return _employeeList;
        }
    }
}
