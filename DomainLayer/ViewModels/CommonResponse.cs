using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.ViewModels
{
    public class GeneralServiceResponse
    {
        public bool success { get; set; }
        public System.Net.HttpStatusCode statusCode { get; set; }
        public string message { get; set; }
        public int totalCount { get; set; }
        public object data { get; set; }
    }
}
