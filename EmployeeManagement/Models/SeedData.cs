using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public static class SeedData
    {
        public static void SeedDataForDb(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee()
                {
                    Id = 1,
                    Name = "Adam",
                    Department = Dept.IT,
                    Email = "Adam@gmail.com"
                },
                new Employee()
                {
                    Id = 2,
                    Name = "Blair",
                    Department = Dept.HR,
                    Email = "Blair@gmail.com"
                }
                );
        }
    }
}
