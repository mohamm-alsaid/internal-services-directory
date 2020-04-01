using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.EntityFrameworkCore;

namespace API_Rel_Pro.Models
{
    public class ContosoContext : DbContext
    {
        public ContosoContext()
        {
        }

        public ContosoContext(DbContextOptions<ContosoContext> options) : base(options)
        {
        }
    }
}