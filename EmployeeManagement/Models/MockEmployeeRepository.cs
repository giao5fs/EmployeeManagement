using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository: IEmployeeRepository
    {

        private List<Employee> employees;
        public MockEmployeeRepository()
        {
            employees = new List<Employee> { 
                new Employee(){Id=1, Name="A", Department="HR", Email="a@a.com"},
                new Employee(){Id=2, Name="B", Department="IT", Email="b@b.com"},
                new Employee(){Id=3, Name="C", Department="PQ", Email="c@a.com"}
            };
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return employees;
        }

        public Employee GetEmployee(int id)
        {
            return employees.FirstOrDefault(x => x.Id == id);
        }
    }
}
