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
    [Route("kalum-management/v1/career-investment")]
    public class CareerInvestmentController : ControllerBase
    {
        private readonly KalumDBContext _kalumDBContext;
        public CareerInvestmentController(KalumDBContext kalumDBContext)
        {
            this._kalumDBContext = kalumDBContext;
        }

        public async Task<ActionResult<CareerInvestment>> Get()
        {
            List<CareerInvestment> careerInvestments = await this._kalumDBContext.CareerInvestments.Include(ci => ci.TechnicalCareer).ToListAsync();
            return Ok(careerInvestments);
        }
    }
}