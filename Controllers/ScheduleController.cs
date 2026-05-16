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
    [Route("kalum-management/v1/schedule")]
    public class ScheduleController : ControllerBase
    {
        private readonly KalumDBContext _kalumDBContext;
        public ScheduleController(KalumDBContext kalumDBContext)
        {
            this._kalumDBContext = kalumDBContext;
        }

        [HttpGet]
        public async Task<ActionResult<Schedule>> Get()
        {
            List<Schedule> schedules = await this._kalumDBContext.Schedules.ToListAsync();
            return Ok(schedules);
        }
        
    }
}