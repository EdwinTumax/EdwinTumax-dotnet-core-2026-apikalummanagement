using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiKalumManagement.DTOs.Requests
{
    public class EnrollmentPaymentUpdateDTO
    {
        [Required]
        public string FileNumber {get;set;}
        [Required]
        public string Year {get;set;}
        [Required]
        public DateTime PaymentDate {get;set;}
        [Required]
        public decimal Amount {get;set;}
    }
}