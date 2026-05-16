using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;

namespace ApiKalumManagement.Entities
{
    public class AdmissionExam
    {
        public string ExamId {get; set;}
        public DateTime ExamDate {get;set;}
        public List<Applicant> Applicants {get;set;}
    }
}