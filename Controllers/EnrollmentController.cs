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
    [Route("kalum-management/v1/enrollment")]
    public class EnrollmentController : ControllerBase
    {

        private readonly KalumDBContext _kalumDBContext;

        public EnrollmentController(KalumDBContext kalumDBContext)
        {
            this._kalumDBContext = kalumDBContext;
        }

        public async Task<ActionResult<Enrollment>> Get()
        {
            List<Enrollment> enrollments = await this._kalumDBContext.Enrollments.Include(e => e.TechnicalCareer).Include(e => e.Schedule).Include(e => e.Student).ToListAsync();
            return Ok(enrollments);
        }
        
    }
}