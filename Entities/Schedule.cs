using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiKalumManagement.Entities
{
    public class Schedule
    {
        public string ScheduleId {get;set;}
        public string Prefix {get;set;}
        public string Description {get;set;}
        public List<Applicant> Applicants {get;set;}
    }
}