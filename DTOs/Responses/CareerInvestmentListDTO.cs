using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiKalumManagement.DTOs.Responses
{
    public class CareerInvestmentListDTO
    {
        public string InvestmentId { get; set; }
        public string CareerId { get; set; }
        public decimal EnrollmentAmount { get; set; }
        public int NumberOfPayment { get; set; }
        public decimal PaymentAmount { get; set; }
    }
}