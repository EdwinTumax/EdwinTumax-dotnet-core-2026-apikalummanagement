using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiKalumManagement.DTOs.Requests
{
    public class AdmissionExamCreatedDTO
    {
        [Required]
        public DateTime ExamDate { get; set; }
    }
}