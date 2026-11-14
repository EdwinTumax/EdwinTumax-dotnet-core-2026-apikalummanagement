using ApiKalumManagement.DBContexts;
using ApiKalumManagement.DTOs.Common;
using ApiKalumManagement.DTOs.Requests;
using ApiKalumManagement.DTOs.Responses;
using ApiKalumManagement.Entities;
using ApiKalumManagement.Services.Interfaces;
using ApiKalumManagement.Shared.Constants;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiKalumManagement.Controllers
{
    [ApiController]
    [Route("kalum-management/v1/enrollment-payments")]
    public class EnrollmentPaymentController : ControllerBase
    {
        private readonly KalumDBContext _kalumDBContext;
        private readonly IMapper _mapper;
        private readonly IUtils _utils;

        public EnrollmentPaymentController(KalumDBContext kalumDBContext, IMapper mapper, IUtils utils)
        {
            this._kalumDBContext = kalumDBContext;
            this._mapper = mapper;
            this._utils = utils;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            long initialTime = DateTime.Now.Ticks;
            List<EnrollmentPayment> enrollmentPayments = await this._kalumDBContext.EnrollmentPayments.ToListAsync();

            if(enrollmentPayments == null || enrollmentPayments.Count == 0)
            {
                this._utils.Log(initialTime, ApiResponses.NoContent.Message, ApiResponses.NoContent.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.GET);
                return NoContent();
            }

            this._utils.Log(initialTime, ApiResponses.Success.Message, ApiResponses.Success.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.GET);
            return Ok(ApiResponseDTO<Object>.Ok(this._mapper.Map<List<EnrollmentPaymentListDTO>>(enrollmentPayments)));
        }

        [HttpGet("{paymentReceipt}", Name = "GetEnrollmentPaymentById")]
        public async Task<ActionResult> GetById(string paymentReceipt)
        {
            long initialTime = DateTime.Now.Ticks;
            EnrollmentPayment enrollmentPayment = await this._kalumDBContext.EnrollmentPayments.FirstOrDefaultAsync(e => e.PaymentReceipt == paymentReceipt);

            if(enrollmentPayment == null)
            {
                this._utils.Log(initialTime, ApiResponses.NotFound.Message, ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.GET);
                return NotFound(ApiResponseDTO<Object>.Fail($"Error de búsqueda con recibo {paymentReceipt}", new List<string>() {$"No existe el pago con el recibo {paymentReceipt}"}));
            }

            this._utils.Log(initialTime, ApiResponses.Success.Message, ApiResponses.Success.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.GET);
            return Ok(ApiResponseDTO<Object>.Ok(this._mapper.Map<EnrollmentPaymentListDTO>(enrollmentPayment)));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] EnrollmentPaymentCreatedDTO enrollmentPaymentCreatedDTO)
        {
            long initialTime = DateTime.Now.Ticks;
            Applicant applicant = await this._kalumDBContext.Applicants.FirstOrDefaultAsync(a => a.FileNumber == enrollmentPaymentCreatedDTO.FileNumber);

            if(applicant == null)
            {
                this._utils.Log(initialTime, ApiResponses.NotFound.Message, ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.POST);
                return NotFound(ApiResponseDTO<Object>.Fail("Crear pago de inscripción.", new List<string>() {$"No existe el aspirante con el expediente {enrollmentPaymentCreatedDTO.FileNumber}"}));
            }

            EnrollmentPayment existingEnrollmentPayment = await this._kalumDBContext.EnrollmentPayments.FirstOrDefaultAsync(e => e.FileNumber == enrollmentPaymentCreatedDTO.FileNumber && e.Year == enrollmentPaymentCreatedDTO.Year);
            if(existingEnrollmentPayment != null)
            {
                this._utils.Log(initialTime, ApiResponses.Conflict.Message, ApiResponses.Conflict.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.POST);
                return Conflict(ApiResponseDTO<Object>.Fail("Crear pago de inscripción.", new List<string>() {$"Ya existe un pago de inscripción de un aspirante con el expediente {enrollmentPaymentCreatedDTO.FileNumber} en el año {enrollmentPaymentCreatedDTO.Year}"}));   
            }

            EnrollmentPayment enrollmentPayment = this._mapper.Map<EnrollmentPayment>(enrollmentPaymentCreatedDTO);
            enrollmentPayment.PaymentReceipt = Guid.NewGuid().ToString();

            await this._kalumDBContext.EnrollmentPayments.AddAsync(enrollmentPayment);
            await this._kalumDBContext.SaveChangesAsync();

            this._utils.Log(initialTime, ApiResponses.Success.Message, ApiResponses.Success.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.POST);
            return CreatedAtRoute("GetEnrollmentPaymentById", new { paymentReceipt = enrollmentPayment.PaymentReceipt }, ApiResponseDTO<Object>.Ok(this._mapper.Map<EnrollmentPaymentListDTO>(enrollmentPayment)));
        }

        [HttpPut("{paymentReceipt}")]
        public async Task<ActionResult> Update(string paymentReceipt, [FromBody] EnrollmentPaymentUpdateDTO enrollmentPaymentUpdateDTO)
        {
            long initialTime = DateTime.Now.Ticks;
            EnrollmentPayment enrollmentPayment = await this._kalumDBContext.EnrollmentPayments.FirstOrDefaultAsync(e => e.PaymentReceipt == paymentReceipt);

            if(enrollmentPayment == null)
            {
                this._utils.Log(initialTime, ApiResponses.NotFound.Message, ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.PUT);
                return NotFound(ApiResponseDTO<Object>.Fail("Actualizar pago de inscripción.", new List<string>() {$"No existe el pago con el recibo {paymentReceipt}"}));
            }

            Applicant applicant = await this._kalumDBContext.Applicants.FirstOrDefaultAsync(a => a.FileNumber == enrollmentPaymentUpdateDTO.FileNumber);

            if(applicant == null)
            {
                this._utils.Log(initialTime, ApiResponses.NotFound.Message, ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.PUT);
                return NotFound(ApiResponseDTO<Object>.Fail("Actualizar pago de inscripción.", new List<string>() {$"No existe el aspirante con el expediente {enrollmentPaymentUpdateDTO.FileNumber}"}));
            }

            this._mapper.Map(enrollmentPaymentUpdateDTO, enrollmentPayment);
            await this._kalumDBContext.SaveChangesAsync();

            this._utils.Log(initialTime, ApiResponses.NoContent.Message, ApiResponses.NoContent.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.PUT);
            return NoContent();
        }

        [HttpDelete("{paymentReceipt}")]
        public async Task<ActionResult> Delete(string paymentReceipt)
        {
            long initialTime = DateTime.Now.Ticks;
            EnrollmentPayment enrollmentPayment = await this._kalumDBContext.EnrollmentPayments.FirstOrDefaultAsync(e => e.PaymentReceipt == paymentReceipt);

            if(enrollmentPayment == null)
            {
                this._utils.Log(initialTime, ApiResponses.NotFound.Message, ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.DELETE);
                return NotFound(ApiResponseDTO<Object>.Fail($"Error al eliminar el pago {paymentReceipt}", new List<string>() {$"No existe el pago con el recibo {paymentReceipt}"}));
            }

            this._kalumDBContext.EnrollmentPayments.Remove(enrollmentPayment);
            await this._kalumDBContext.SaveChangesAsync();

            this._utils.Log(initialTime, ApiResponses.NoContent.Message, ApiResponses.NoContent.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.DELETE);
            return NoContent();
        }
    }
}