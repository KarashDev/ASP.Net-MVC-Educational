using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Net_MVC_Educational.Models
{
    public class DataBaseCreator
    {
        public static void CreateDataBase(CarStoreDbContext context)
        {
            if (!context.Cars.Any())
            {
                context.Cars.AddRange(
                    new Car
                    {
                        Name = "IX5",
                        Company = "BMW",
                        Price = 600500
                    },
                    new Car
                    {
                        Name = "Kalina",
                        Company = "Lada",
                        Price = 550000
                    },
                    new Car
                    {
                        Name = "Focus",
                        Company = "Ford",
                        Price = 450000
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
