using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiKalumManagement.Entities
{
    public class CareerInvestment
    {
        public string InvestmentId {get;set;}
        public string CareerId {get;set;}
        public decimal EnrollmentAmount {get;set;}
        public int NumberOfPayment {get;set;}
        public decimal PaymentAmount {get;set;}
        public TechnicalCareer TechnicalCareer {get;set;}
    }
}