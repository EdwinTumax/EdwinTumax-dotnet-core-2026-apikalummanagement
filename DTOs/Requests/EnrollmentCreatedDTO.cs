using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiKalumManagement.DTOs.Requests
{
    public class EnrollmentCreatedDTO
    {
        [Required]
        public string StudentId {get;set;}
        [Required]
        public string CareerId {get;set;}
        [Required]
        public string ScheduleId {get;set;}
        [Required]
        public string Cycle {get;set;}
        [Required]
        public DateTime EnrollmentDate {get;set;}
    }
}