﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{

    /// <summary>
    /// Extend Identity User
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public string City { get; set; }
    }
}
