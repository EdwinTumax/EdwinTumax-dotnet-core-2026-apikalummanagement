using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiKalumManagement.DTOs.Requests
{
    public class ChargeUpdateDTO
    {
        [Required]
        public string Description {get;set;}
        [Required]
        public string Prefix {get;set;}
        [Required]
        public decimal Amount {get;set;}
        public bool GeneratesLateFee {get;set;}
        public int LateFeePercentage {get;set;}
    }
}