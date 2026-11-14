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
    [Route("kalum-management/v1/enrollments")]
    public class EnrollmentController : ControllerBase
    {
        private readonly KalumDBContext _kalumDBContext;
        private readonly IMapper _mapper;
        private readonly IUtils _utils;

        public EnrollmentController(KalumDBContext kalumDBContext, IMapper mapper, IUtils utils)
        {
            this._kalumDBContext = kalumDBContext;
            this._mapper = mapper;
            this._utils = utils;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            long initialTime = DateTime.Now.Ticks;
            List<Enrollment> enrollments = await this._kalumDBContext.Enrollments.Include(e => e.Student) .ToListAsync();

            if(enrollments == null || enrollments.Count == 0)
            {
                this._utils.Log(initialTime, ApiResponses.NoContent.Message, ApiResponses.NoContent.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.GET);
                return NoContent();
            }

            this._utils.Log(initialTime, ApiResponses.Success.Message, ApiResponses.Success.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.GET);
            return Ok(ApiResponseDTO<Object>.Ok(this._mapper.Map<List<EnrollmentListDTO>>(enrollments)));
        }

        [HttpGet("{enrollmentId}", Name = "GetEnrollmentById")]
        public async Task<ActionResult> GetById(string enrollmentId)
        {
            long initialTime = DateTime.Now.Ticks;
            Enrollment enrollment = await this._kalumDBContext.Enrollments.FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId);

            if(enrollment == null)
            {
                this._utils.Log(initialTime, ApiResponses.NotFound.Message, ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.GET);
                return NotFound(ApiResponseDTO<Object>.Fail($"Error de búsqueda con matrícula {enrollmentId}", new List<string>() {$"No existe la matrícula con el id {enrollmentId}"}));
            }

            this._utils.Log(initialTime, ApiResponses.Success.Message, ApiResponses.Success.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.GET);
            return Ok(ApiResponseDTO<Object>.Ok(this._mapper.Map<EnrollmentListDTO>(enrollment)));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] EnrollmentCreatedDTO enrollmentCreatedDTO)
        {
            long initialTime = DateTime.Now.Ticks;

            Student student = await this._kalumDBContext.Students.FirstOrDefaultAsync(s => s.StudentId == enrollmentCreatedDTO.StudentId);

            if(student == null)
            {
                this._utils.Log(initialTime, ApiResponses.NotFound.Message, ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.POST);
                return NotFound(ApiResponseDTO<Object>.Fail("Crear matrícula.", new List<string>() {$"No existe el estudiante con el id {enrollmentCreatedDTO.StudentId}"}));
            }

            TechnicalCareer technicalCareer = await this._kalumDBContext.TechnicalCareers.FirstOrDefaultAsync(c => c.CareerId == enrollmentCreatedDTO.CareerId);

            if(technicalCareer == null)
            {
                this._utils.Log(initialTime, ApiResponses.NotFound.Message, ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.POST);
                return NotFound(ApiResponseDTO<Object>.Fail("Crear matrícula.", new List<string>() {$"No existe la carrera técnica con el id {enrollmentCreatedDTO.CareerId}"}));
            }

            Schedule schedule = await this._kalumDBContext.Schedules.FirstOrDefaultAsync(s => s.ScheduleId == enrollmentCreatedDTO.ScheduleId);

            if(schedule == null)
            {
                this._utils.Log(initialTime, ApiResponses.NotFound.Message, ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.POST);
                return NotFound(ApiResponseDTO<Object>.Fail("Crear matrícula.", new List<string>() {$"No existe el horario con el id {enrollmentCreatedDTO.ScheduleId}"}));
            }

            Enrollment enrollment = this._mapper.Map<Enrollment>(enrollmentCreatedDTO);
            enrollment.EnrollmentId = Guid.NewGuid().ToString();

            await this._kalumDBContext.Enrollments.AddAsync(enrollment);
            await this._kalumDBContext.SaveChangesAsync();

            this._utils.Log(initialTime, ApiResponses.Success.Message, ApiResponses.Success.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.POST);
            return CreatedAtRoute("GetEnrollmentById", new { enrollmentId = enrollment.EnrollmentId }, ApiResponseDTO<Object>.Ok(this._mapper.Map<EnrollmentListDTO>(enrollment)));
        }

        [HttpPut("{enrollmentId}")]
        public async Task<ActionResult> Update(string enrollmentId, [FromBody] EnrollmentUpdateDTO enrollmentUpdateDTO)
        {
            long initialTime = DateTime.Now.Ticks;
            Enrollment enrollment = await this._kalumDBContext.Enrollments.FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId);

            if(enrollment == null)
            {
                this._utils.Log(initialTime, ApiResponses.NotFound.Message, ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.PUT);
                return NotFound(ApiResponseDTO<Object>.Fail("Actualizar matrícula.", new List<string>() {$"No existe la matrícula con el id {enrollmentId}"}));
            }

            Student student = await this._kalumDBContext.Students.FirstOrDefaultAsync(s => s.StudentId == enrollmentUpdateDTO.StudentId);

            if(student == null)
            {
                this._utils.Log(initialTime, ApiResponses.NotFound.Message, ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.PUT);
                return NotFound(ApiResponseDTO<Object>.Fail("Actualizar matrícula.", new List<string>() {$"No existe el estudiante con el id {enrollmentUpdateDTO.StudentId}"}));
            }

            TechnicalCareer technicalCareer = await this._kalumDBContext.TechnicalCareers.FirstOrDefaultAsync(c => c.CareerId == enrollmentUpdateDTO.CareerId);

            if(technicalCareer == null)
            {
                this._utils.Log(initialTime, ApiResponses.NotFound.Message, ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.PUT);
                return NotFound(ApiResponseDTO<Object>.Fail("Actualizar matrícula.", new List<string>() {$"No existe la carrera técnica con el id {enrollmentUpdateDTO.CareerId}"}));
            }

            Schedule schedule = await this._kalumDBContext.Schedules.FirstOrDefaultAsync(s => s.ScheduleId == enrollmentUpdateDTO.ScheduleId);

            if(schedule == null)
            {
                this._utils.Log(initialTime, ApiResponses.NotFound.Message, ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.PUT);
                return NotFound(ApiResponseDTO<Object>.Fail("Actualizar matrícula.", new List<string>() {$"No existe el horario con el id {enrollmentUpdateDTO.ScheduleId}"}));
            }

            this._mapper.Map(enrollmentUpdateDTO, enrollment);
            await this._kalumDBContext.SaveChangesAsync();

            this._utils.Log(initialTime, ApiResponses.NoContent.Message, ApiResponses.NoContent.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.PUT);
            return NoContent();
        }

        [HttpDelete("{enrollmentId}")]
        public async Task<ActionResult> Delete(string enrollmentId)
        {
            long initialTime = DateTime.Now.Ticks;
            Enrollment enrollment = await this._kalumDBContext.Enrollments.FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId);

            if(enrollment == null)
            {
                this._utils.Log(initialTime, ApiResponses.NotFound.Message, ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.DELETE);
                return NotFound(ApiResponseDTO<Object>.Fail($"Error al eliminar la matrícula {enrollmentId}", new List<string>() {$"No existe la matrícula con el id {enrollmentId}"}));
            }

            this._kalumDBContext.Enrollments.Remove(enrollment);
            await this._kalumDBContext.SaveChangesAsync();

            this._utils.Log(initialTime, ApiResponses.NoContent.Message, ApiResponses.NoContent.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.DELETE);
            return NoContent();
        }
    }
}