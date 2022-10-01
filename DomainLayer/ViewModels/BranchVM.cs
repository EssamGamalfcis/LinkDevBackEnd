using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.ViewModels
{
    public class BranchVM
    {
        public long? Id { get; set; }
        [MaxLength(200, ErrorMessage = "Title length should not exceed 200 characters")]
        public string title { get; set; }
        public string openingHourString { get; set; }
        public string closingHourString { get; set; }
        public string openingHour { get; set; }
        public string closingHour { get; set; }
        public bool? canBook { get; set; }
        [MaxLength(250, ErrorMessage = "Manager Name length should not exceed 250 characters")]
        public string managerName { get; set; }
    }
}
