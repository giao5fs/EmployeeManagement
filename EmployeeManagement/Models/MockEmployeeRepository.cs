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
                new Employee(){Id=1, Name="Alan Sheerer", Department=Dept.HR, Email="a@a.com"},
                new Employee(){Id=2, Name="Bobee Charton", Department=Dept.IT, Email="b@b.com"},
                new Employee(){Id=3, Name="Container", Department=Dept.PR, Email="c@a.com"}
            };
        }

        public Employee Add(Employee employee)
        {
            employee.Id = employees.Max(x => x.Id) + 1;
            employees.Add(employee);
            return employee;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return employees;
        }

        public Employee GetEmployee(int id)
        {
            return employees.FirstOrDefault(x => x.Id == id);
        }

        public Employee Update(Employee employee)
        {
            Employee emp = employees.FirstOrDefault(x => x.Id == employee.Id);
            if (emp != null)
            {
                emp.Name = employee.Name;
                emp.Email = employee.Email;
                emp.Department = employee.Department;
            }
            return emp;
        }

        public Employee Delete(int id)
        {
            Employee emp = employees.FirstOrDefault(x => x.Id == id);
            if (emp != null)
            {
                employees.Remove(emp);
            }
            return emp;
        }
    }
}
