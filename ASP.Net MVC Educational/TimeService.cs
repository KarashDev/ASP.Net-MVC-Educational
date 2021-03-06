using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Net_MVC_Educational
{
    public interface ITimeService
    {
        string Time { get; }
    }
    public class SimpleTimeService : ITimeService
    {
        public string Time { get; }
        
        public SimpleTimeService()
        {
            Time = DateTime.Now.ToString("hh:mm:ss");
        }
    }
}
