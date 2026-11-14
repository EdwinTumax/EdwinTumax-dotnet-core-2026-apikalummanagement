using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiKalumManagement.DTOs.Requests
{
    public class AdmissionExamResultUpdateDTO
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public int Score { get; set; }
    }
}