using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace autocarrs.Models
{
    public class OrderMaster
    {
        [Key]
        [Column(Order = 1)]
        public int INVNUM { get; set; }
        [Key]
        [Column(Order = 2)]
        public DateTime TRDATE { get; set; }
        [StringLength(30)]
        public string BUYRID { get; set; }
        public int AMOUNT { get; set; }
        public int PartialPayment { get; set; }
    }
}