using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Net_MVC_Educational.Models
{
    public class CarStoreDbContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Order> Orders { get; set; }

        public CarStoreDbContext(DbContextOptions<CarStoreDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
