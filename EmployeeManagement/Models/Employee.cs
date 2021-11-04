using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(10, ErrorMessage ="Name cannot exceed 10 character")]
        public string Name { get; set; }
        [Required]
        [Display(Name="Office Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public Dept Department { get; set; }
        public String PhotoPath { get; set; }
    }
}
