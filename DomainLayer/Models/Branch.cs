using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class Branch : BaseEntity
    {
        [MaxLength(200 ,ErrorMessage = "Title length should not exceed 200 characters")]
        public string Title { get; set;}
        public long OpeningHour { get; set;}
        public long ClosingHour { get; set;}
        [MaxLength(250, ErrorMessage = "Manager Name length should not exceed 250 characters")]
        public string ManagerName { get; set; }
    }
}
