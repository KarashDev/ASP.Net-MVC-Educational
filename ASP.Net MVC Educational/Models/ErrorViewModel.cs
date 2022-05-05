using System;

namespace ASP.Net_MVC_Educational.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
