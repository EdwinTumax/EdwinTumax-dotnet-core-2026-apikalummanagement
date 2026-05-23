using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiKalumManagement.Entities
{
    public class Charge
    {
        public string ChargeId {get;set;}
        public string Description {set;get;}
        public string Prefix {get;set;}
        public decimal Amount {set;get;}
        public bool GeneratesLateFee {get;set;}
        public int LateFeePercentage {get;set;} 
        public List<AccountsReceivable> AccountsReceivables {get;set;}
    }   
}