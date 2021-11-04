using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class SqlEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext dbContext;

        public SqlEmployeeRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Employee Add(Employee employee)
        {
            dbContext.Employees.Add(employee);
            dbContext.SaveChanges();
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee emp = dbContext.Employees.Find(id);
            if (emp!=null)
            {
                dbContext.Employees.Remove(emp);
                dbContext.SaveChanges();
            }
            return emp;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return dbContext.Employees.ToList();
        }

        public Employee GetEmployee(int id)
        {
            Employee emp = dbContext.Employees.Find(id);
            return emp;
        }

        public Employee Update(Employee employee)
        {
            var emp = dbContext.Employees.Attach(employee);
            emp.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            dbContext.SaveChanges();
            return employee;
        }
    }
}
