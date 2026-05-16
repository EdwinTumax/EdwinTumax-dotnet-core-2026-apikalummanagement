using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiKalumManagement.DBContexts;
using ApiKalumManagement.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiKalumManagement.Controllers
{
    [ApiController]
    [Route("kalum-management/v1/addmission-exam-result")]
    public class AdmissionExamResultController : ControllerBase
    {
        
        private readonly KalumDBContext _kalumDBContext;

        public AdmissionExamResultController(KalumDBContext kalumDBContext)
        {
            this._kalumDBContext = kalumDBContext;
        }

        [HttpGet]
        public async Task<ActionResult<AdmissionExamResult>> Get()
        {
            List<AdmissionExamResult> admissionExamResults = await this._kalumDBContext.AdmissionExamResults.ToListAsync();
            return Ok(admissionExamResults);            
        }
    }
}