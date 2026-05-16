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
        public List<Applicant> applicants {get;set;}
    }
}