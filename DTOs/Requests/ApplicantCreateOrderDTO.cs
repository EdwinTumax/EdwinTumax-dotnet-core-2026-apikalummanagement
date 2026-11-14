using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiKalumManagement.DTOs.Requests
{
    public class ApplicantCreateOrderDTO
    {
        public string OrderId {get;set;}
        public DateTime OrderDate {get;set;}
        public string Status {get;set;}
        public ApplicantCreatedDTO Data {get;set;}   
    }
}