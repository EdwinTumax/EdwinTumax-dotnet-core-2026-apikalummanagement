using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiKalumManagement.Entities
{
    public class Enrollment
    {
        public string EnrollmentId {get;set;}
        public string StudentId {get;set;}
        public string CareerId {get;set;}
        public string ScheduleId {get;set;}
        public string Cycle {get;set;}
        public DateTime EnrollmentDate {get;set;}
        public Schedule Schedule {get;set;}
        public TechnicalCareer TechnicalCareer {get;set;}
        public Student Student {set;get;}
    }
}