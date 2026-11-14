
namespace ApiKalumManagement.Entities
{
    public class AdmissionExam
    {
        public string ExamId {get; set;}
        public DateTime ExamDate {get;set;}
        public List<Applicant> Applicants {get;set;}
    }
}