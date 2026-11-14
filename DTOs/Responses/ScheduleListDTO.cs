using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiKalumManagement.DTOs.Responses
{
    public class ScheduleListDTO
    {
        public string ScheduleId { get; set; }
        public string Prefix { get; set; }
        public string Description { get; set; }
    }
}