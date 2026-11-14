using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiKalumManagement.DTOs.Requests
{
    public class CareerInvestmentCreatedDTO
    {
        [Required]
        public string CareerId {get;set;}
        [Required]
        public decimal EnrollmentAmount {get;set;}
        [Required]
        public int NumberOfPayment {get;set;}
        [Required]
        public decimal PaymentAmount {get;set;}
    }
}