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
    [Route("kalum-management/v1/charge")]
    public class ChargeController : ControllerBase
    {
        private readonly KalumDBContext _kalumDBContext;
        public ChargeController(KalumDBContext kalumDBContext)
        {
            this._kalumDBContext = kalumDBContext;
        }

        [HttpGet]
        public async Task<ActionResult<Charge>> Get()
        {
            List<Charge> charges = await this._kalumDBContext.Charges.Include(ar => ar.AccountsReceivables).ToListAsync();
            return Ok(charges);
        }
        
    }
}