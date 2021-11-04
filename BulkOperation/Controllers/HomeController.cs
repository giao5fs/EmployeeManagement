using BulkOperation.Models;
using BulkOperation.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BulkOperation.Controllers
{
    [Route("api/{controller}")]
    public class HomeController : Controller
    {
        private readonly AppDbContext repository;
        private readonly EmployeeServices services;

        public HomeController(EmployeeServices services)
        {
            this.services = services;
        }
        [HttpPost(nameof(BulkAdd))]
        public async Task<IActionResult> BulkAdd()
        {
            var response = await services.AddBulkAsyncData();
            return Ok(response);
        }
        [HttpPut(nameof(BulkUpdate))]
        public async Task<IActionResult> BulkUpdate()
        {
            var response = await services.UpdateBulkAsyncData();
            return Ok(response);
        }
        [HttpDelete(nameof(BulkDelete))]
        public async Task<IActionResult> BulkDelete()
        {
            var response = await services.DeleteBulkAsyncData();
            return Ok(response);
        }

    }
}
