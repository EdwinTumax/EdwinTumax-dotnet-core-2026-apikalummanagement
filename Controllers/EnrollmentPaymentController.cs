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
    [Route("kalum-management/v1/enrollment-payment")]
    public class EnrollmentPaymentController : ControllerBase
    {
        private readonly KalumDBContext _kalumDBContext;

        public EnrollmentPaymentController(KalumDBContext kalumDBContext)
        {
            this._kalumDBContext = kalumDBContext;
        }

        public async Task<ActionResult<EnrollmentPayment>> Get()
        {
            List<EnrollmentPayment> enrollmentPayments = await this._kalumDBContext.EnrollmentPayments.ToListAsync();
            return Ok(enrollmentPayments);
        }
    }
}