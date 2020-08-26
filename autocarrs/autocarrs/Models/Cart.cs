using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace autocarrs.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        public int AutoId { get; set; }
        [StringLength(30)]
        public string BuyrID { get; set; } //if BuyrId is null, generate a one
        public int SellPri { get; set; }
        [StringLength(1)]
        public string Status { get; set; } //'T, temporary 
        public DateTime Endate { get; set; }
        public AutosVehicle AutosVehicle { get; set; }
    }
}