using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiKalumManagement.DTOs.Responses
{
    public class EnrollmentPaymentListDTO
    {
        public string PaymentReceipt {get;set;}
        public string FileNumber {get;set;}
        public string Year {get;set;}
        public DateTime PaymentDate {get;set;}
        public decimal Amount {get;set;}
    }
}