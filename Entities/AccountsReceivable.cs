using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace ApiKalumManagement.Entities
{
    public class AccountsReceivable
    {
        public string ChargeName {get;set;}
        public string Year {get;set;}
        public string StudentId {set;get;}
        public string Description {get;set;}
        public DateTime ChargeDate {get;set;}
        public DateTime ApplyDate {get;set;}
        public decimal Amount {get;set;}
        public decimal LateFee {get;set;}
        public decimal Discount {get;set;}
        public string ChargeId {get;set;}
        public Charge Charge {get;set;}
        public Student Student {get;set;}
    }
}