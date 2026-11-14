using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiKalumManagement.DTOs.Requests
{
    public class TechnicalCareerCreatedDTO
    {
        [Required]
        public string Name {get;set;}
        [Required]
        public string Description {get; set;}
        [Required]
        public string SubTitle {get;set;}
        [Required]
        public string Image {get;set;}
    }
}