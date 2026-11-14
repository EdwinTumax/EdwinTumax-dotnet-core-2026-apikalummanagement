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
    [Route("kalum-management/v1/applicants")]
    public class ApplicantController : ControllerBase
    {
        private readonly KalumDBContext _kalumDBContext;
        private readonly IMapper _mapper;
        private readonly IUtils _utils;
        private readonly IRabbitExchangeService _rabbitExchangeService;

        public ApplicantController(KalumDBContext kalumDBContext, IMapper mapper, IUtils utils, IRabbitExchangeService rabbitExchangeService)
        {
            this._kalumDBContext = kalumDBContext;
            this._mapper = mapper;
            this._utils = utils;
            this._rabbitExchangeService = rabbitExchangeService;
        }

        [HttpGet]
        public async Task<ActionResult<ApplicantListDTO>> Get()        
        {
            long initialTime = DateTime.Now.Ticks;
            List<Applicant> applicants = await this._kalumDBContext.Applicants
                .Include(a => a.AdmissionExam)
                .Include(a => a.Schedule)
                .Include(a => a.TechnicalCareer).ToListAsync();
            if (applicants == null || applicants.Count == 0)
            {
                this._utils.Log(initialTime,ApiResponses.NoContent.Message,ApiResponses.NoContent.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.GET);
                return NoContent();
            }
            this._utils.Log(initialTime,ApiResponses.Success.Message,ApiResponses.Success.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.GET);
            return Ok(ApiResponseDTO<Object>.Ok(this._mapper.Map<List<ApplicantListDTO>>(applicants)));
        }

        [HttpGet("{applicantId}", Name = "GetApplicantById")]
        public async Task<ActionResult<ApplicantListDTO>> GetById(string applicantId)
        {
            long initialTime = DateTime.Now.Ticks;
            Applicant applicant = await this._kalumDBContext.Applicants
                .Include(a => a.AdmissionExam)
                .Include(a => a.Schedule)
                .Include(a => a.TechnicalCareer)
                .FirstOrDefaultAsync(a => a.FileNumber == applicantId);
            if (applicant == null)
            {
                this._utils.Log(initialTime,ApiResponses.NotFound.Message,ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.GET);
                return NotFound(ApiResponseDTO<Object>.Fail($"Error de busqueda con expediente {applicantId}", new List<string>() {$"No existe el expediente con el id {applicantId}"}));
            }
            this._utils.Log(initialTime,ApiResponses.Success.Message,ApiResponses.Success.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.GET);
            return Ok(ApiResponseDTO<Object>.Ok(this._mapper.Map<ApplicantListDTO>(applicant)));
        }

        [HttpGet("search")]
        public async Task<ActionResult<ApplicantListDTO>> SearchByEmail([FromQuery] string email)
        {
            long initialTime = DateTime.Now.Ticks;
            Applicant applicant = await this._kalumDBContext.Applicants.FirstOrDefaultAsync(a => a.Email.ToLower() == email.ToLower());
            if(applicant == null) 
            {
                this._utils.Log(initialTime,ApiResponses.NotFound.Message,ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.GET);
                return NotFound(ApiResponseDTO<Object>.Fail($"Error de busqueda por email", new List<string>() {$"No existe el Email {email}"}));
            }
            this._utils.Log(initialTime,ApiResponses.Success.Message,ApiResponses.Success.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.GET);
            return Ok(ApiResponseDTO<Object>.Ok(this._mapper.Map<ApplicantListDTO>(applicant)));     
        } 

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ApplicantCreatedDTO applicantCreatedDTO)
        {
            long initialTime = DateTime.Now.Ticks;
            ApplicantCreateOrderDTO order = new ApplicantCreateOrderDTO();
            TechnicalCareer technicalCareer = await this._kalumDBContext.TechnicalCareers.FirstOrDefaultAsync(tc => tc.CareerId == applicantCreatedDTO.CareerId);
            if(technicalCareer == null)
            {
                this._utils.Log(initialTime,ApiResponses.NotFound.Message,ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.POST);
                return NotFound(ApiResponseDTO<Object>.Fail("Crear aspirante.", new List<string>() {$"No existe la carrera técnica con el id {applicantCreatedDTO.CareerId}"}));                
            }
            Schedule schedule = await this._kalumDBContext.Schedules.FirstOrDefaultAsync(s => s.ScheduleId == applicantCreatedDTO.ScheduleId);
            if(schedule == null)
            {
                this._utils.Log(initialTime,ApiResponses.NotFound.Message,ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.POST);
                return NotFound(ApiResponseDTO<Object>.Fail("Crear aspirante.", new List<string>() {$"No existe la jornada con el id {applicantCreatedDTO.ScheduleId}"}));                
                
            }
            AdmissionExam admissionExam = await this._kalumDBContext.AdmissionExams.FirstOrDefaultAsync(ae => ae.ExamId == applicantCreatedDTO.ExamId);
            if(admissionExam == null)
            {
                this._utils.Log(initialTime,ApiResponses.NotFound.Message,ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.POST);
                return NotFound(ApiResponseDTO<Object>.Fail("Crear aspirante.", new List<string>() {$"No existe la fecha de examen con el id {applicantCreatedDTO.ExamId}"}));                                
            }
            order.OrderId = Guid.NewGuid().ToString();
            order.OrderDate = DateTime.Now;
            order.Status = "IN_PROGRESS";
            order.Data = applicantCreatedDTO;
            bool result = await this._rabbitExchangeService.ApplicantCreatedOrderAsync(order);
            this._utils.Log(initialTime, ApiResponses.Accepted.Message, ApiResponses.Accepted.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.POST);
            return StatusCode(202, ApiResponseDTO<Object>.Ok(order));
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, [FromBody] ApplicantUpdateDTO applicantUpdateDTO)
        {
            long initialTime = DateTime.Now.Ticks;
            Applicant applicant = await this._kalumDBContext.Applicants.FirstOrDefaultAsync(a => a.FileNumber == id );  
            if(applicant == null)
            {
                this._utils.Log(initialTime,ApiResponses.NotFound.Message,ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.GET);
                return NotFound(ApiResponseDTO<Object>.Fail("Actualizar aspirante.", new List<string>() {$"No existe aspirante con el id {id}"}));
            }
            Applicant update = this._mapper.Map(applicantUpdateDTO,applicant);            
            await this._kalumDBContext.SaveChangesAsync();
            this._utils.Log(initialTime,ApiResponses.NoContent.Message,ApiResponses.NoContent.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.GET);    
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            long initialTime = DateTime.Now.Ticks;
            Applicant applicant = await this._kalumDBContext.Applicants.FirstOrDefaultAsync(a => a.FileNumber == id);
            if(applicant == null)
            {
                this._utils.Log(initialTime,ApiResponses.NotFound.Message,ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.GET);
                return NotFound(ApiResponseDTO<Object>.Fail($"Error al eliminar el expediente {id}", new List<string>(){$"No existe el aspirante con el número de expediente {id}"}));
            }
            this._kalumDBContext.Applicants.Remove(applicant);
            await this._kalumDBContext.SaveChangesAsync();
            this._utils.Log(initialTime,ApiResponses.NoContent.Message,ApiResponses.NoContent.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.GET);
            return NoContent(); 
        }

    }
}