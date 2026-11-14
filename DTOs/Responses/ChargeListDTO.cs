using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiKalumManagement.DTOs.Responses
{
    public class ChargeListDTO
    {
        public string ChargeId { get; set; }
        public string Description { get; set; }
        public string Prefix { get; set; }
        public decimal Amount { get; set; }
        public bool GeneratesLateFee { get; set; }
        public int LateFeePercentage { get; set; }
    }
}