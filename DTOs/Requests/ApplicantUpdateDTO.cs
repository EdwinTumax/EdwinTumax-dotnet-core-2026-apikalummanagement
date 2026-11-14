using System.ComponentModel.DataAnnotations;

namespace ApiKalumManagement.DTOs.Requests
{
    public class ApplicantUpdateDTO
    {
        [Required]
        public string LastName {get;set;}
        [Required]
        public string FirstName {get;set;}
        [Required]
        public string Address {get;set;}
        public string Phone {get;set;}
        [Required]
        [EmailAddress]
        public string Email {get;set;}
        [Required]
        public string ExamId {get;set;}
        [Required]
        public string ScheduleId {get;set;}
        [Required]
        public string CareerId {get;set;}
    }
}