using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiKalumManagement.Entities
{
    public class TechnicalCareer
    {
        public string CareerId {get;set;}
        public string Name {get;set;}
        public string Image {get;set;}
        public string Description {get;set;}
        public string SubTitle {get;set;}
        public List<Applicant> Applicants {get;set;}
        public List<Enrollment> Enrollments {get;set;}
        public List<CareerInvestment> CareerInvestments {get;set;}
    }
}