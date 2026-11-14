using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiKalumManagement.DTOs.Responses
{
    public class TechnicalCareerListDTO
    {
        public string CareerId { get; set; }
        public string Name { get; set; }
        public string Image {get;set;}
        public string Description {get;set;}
        public string SubTitle {get;set;}
    }
}