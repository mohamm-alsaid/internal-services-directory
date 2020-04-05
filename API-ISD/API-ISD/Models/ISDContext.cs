using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.EntityFrameworkCore;

namespace API_ISD.Models
{
    public class ISDContext: DbContext
    {
        public ISDContext() { }

        public ISDContext(DbContextOptions<ISDContext> options) : base(options) { }
    }
}