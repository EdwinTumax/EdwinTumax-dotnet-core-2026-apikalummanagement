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
    [Route("kalum-management/v1/applicant")]
    public class ApplicantController : ControllerBase
    {
        private readonly KalumDBContext _kalumDBContext;
        
        public ApplicantController(KalumDBContext kalumDBContext)
        {
            this._kalumDBContext = kalumDBContext;
        }

        [HttpGet]
        public async Task<ActionResult<Applicant>> Get()
        {
            List<Applicant> applicants = await this._kalumDBContext.Applicants
                .Include(a => a.AdmissionExam)
                .Include(a => a.Schedule)
                .Include(a => a.TechnicalCareer)
                .Include(a => a.AdmissionExamResults)
                .Include(a => a.EnrollmentPayments).ToListAsync();
            return Ok(applicants);
        }
        
        [HttpGet("{fileNumber}")]
        public async Task<ActionResult<Applicant>> GetById(string fileNumber)
        {
            Applicant applicant = await this._kalumDBContext.Applicants.FirstOrDefaultAsync(a => a.FileNumber == fileNumber);          
            if(applicant == null)
            {
                return BadRequest();
            } 
            return Ok(applicant);
        }

    }
}