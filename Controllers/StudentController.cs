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
    [Route("kalum-management/v1/student")]
    public class StudentController : ControllerBase
    {
        private readonly KalumDBContext _kalumDBContext;

        public StudentController(KalumDBContext kalumDBContext)
        {
            this._kalumDBContext = kalumDBContext;
        }

        public async Task<ActionResult<Student>> Get()
        {
            List<Student> students = await this._kalumDBContext.Students.Include(e => e.Enrollments).ToListAsync();
            return Ok(students);
        }
    }
}