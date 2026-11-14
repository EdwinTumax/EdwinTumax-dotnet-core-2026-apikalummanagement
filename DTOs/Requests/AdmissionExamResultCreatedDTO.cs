using System.ComponentModel.DataAnnotations;

namespace ApiKalumManagement.DTOs.Requests
{
    public class AdmissionExamResultCreatedDTO
    {
        [Required]
        public string FileNumber { get; set; }
        [Required]
        public string Year { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Score { get; set; }
    }
}
