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
    [Route("kalum-management/v1/technical-career")]
    public class TechnicalCareerController : ControllerBase
    {
        private readonly KalumDBContext _kalumDBContext;

        public TechnicalCareerController(KalumDBContext kalumDBContext)
        {
            this._kalumDBContext = kalumDBContext;
        }
        
        [HttpGet]
        public async Task<ActionResult<TechnicalCareer>> Get()
        {
            List<TechnicalCareer> technicalCareers = await this._kalumDBContext.TechnicalCareers.ToListAsync();
            return Ok(technicalCareers);
        }
    }
}