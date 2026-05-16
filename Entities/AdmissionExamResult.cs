using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiKalumManagement.Entities
{
    public class AdmissionExamResult
    {
        public string FileNumber {get;set;}
        public string Year {get;set;}
        public string Description {get;set;}
        public int Score {get;set;}
        public Applicant Applicant {get;set;}
    }
}