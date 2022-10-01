using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.ViewModels
{
    public class LoginResponse
    {
        public string userId { get; set; }
        public string name { get; set; }
        public string token { get; set; }
        public List<string> roles { get; set; }
    }

}
