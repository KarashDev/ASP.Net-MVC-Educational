using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Net_MVC_Educational.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string User { get; set; } 
        public string Address { get; set; } 
        public string ContactPhone { get; set; } 

        public int CarId { get; set; } 
        public Car Car { get; set; }
    }
}
