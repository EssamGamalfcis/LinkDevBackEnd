using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class UserBranchBooking :BaseEntity
    {
        public Branch Branch { get; set; }
        public IdentityUser User { get; set; }
    }
}
