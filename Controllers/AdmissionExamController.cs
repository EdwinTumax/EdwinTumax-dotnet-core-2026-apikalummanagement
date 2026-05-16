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
    [Route("kalum-management/v1/addmission-exam")]
    public class AdmissionExamController : ControllerBase
    {
        private readonly KalumDBContext _kalumDBContext;
        public AdmissionExamController(KalumDBContext kalumDBContext)
        {
            this._kalumDBContext = kalumDBContext;
        }

        [HttpGet]
        public async Task<ActionResult<AdmissionExam>> Get()
        {
            List<AdmissionExam> admissionExams = await this._kalumDBContext.AdmissionExams.ToListAsync();
            return Ok(admissionExams);
        }
    }
}