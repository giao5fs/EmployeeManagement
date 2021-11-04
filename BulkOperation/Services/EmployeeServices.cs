using BulkOperation.Models;
using EFCore.BulkExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BulkOperation.Services
{
    public class EmployeeServices
    {
        private readonly AppDbContext dbContext;
        DateTime Start;
        TimeSpan TimeSpan;
        public EmployeeServices(AppDbContext repository)
        {
            this.dbContext = repository;
        }

        public async Task<TimeSpan> AddBulkAsyncData()
        {
            Start = DateTime.Now;
            List<Employee> employees = new();
            for (int i = 0; i < 100000; i++)
            {
                employees.Add(new Employee() { Name = $"AAAAAA {i}", Email = $"BBBBB {i}" });
            }

            await dbContext.BulkInsertAsync(employees);
            TimeSpan = DateTime.Now - Start;
            return TimeSpan;
        }

        public async Task<TimeSpan> UpdateBulkAsyncData()
        {
            Start = DateTime.Now;
            List<Employee> employees = new();
            for (int i = 0; i < 100000; i++)
            {
                employees.Add(new Employee() { Name = $"AAAAAA {i}", Email = $"BBBBB {i}" });
            }

            await dbContext.BulkUpdateAsync(employees);
            TimeSpan = DateTime.Now - Start;
            return TimeSpan;
        }

        public async Task<TimeSpan> DeleteBulkAsyncData()
        {
            Start = DateTime.Now;
            List<Employee> employees = new();
            employees = dbContext.Employees.ToList();
            await dbContext.BulkDeleteAsync(employees);
            TimeSpan = DateTime.Now - Start;
            return TimeSpan;
        }
    }
}
