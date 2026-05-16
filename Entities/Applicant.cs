using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiKalumManagement.Entities
{
    public class Applicant
    {
        public string FileNumber {get;set;}
        public string LastName {get;set;}
        public string FirstName {get;set;}
        public string Address {get;set;}
        public string Phone {get;set;}
        public string Email {get;set;}
        public string Status {get;set;}
        public string ExamId {get;set;}
        public string ScheduleId {get;set;}
        public string CareerId {get;set;}
        public AdmissionExam AdmissionExam {get;set;}
        public Schedule Schedule {get;set;}
        public TechnicalCareer TechnicalCareer {get;set;}
    }
}