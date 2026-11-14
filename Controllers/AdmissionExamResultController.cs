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
    [Route("kalum-management/v1/addmission-exam-results")]
    public class AdmissionExamResultController : ControllerBase
    {

        private readonly KalumDBContext _kalumDBContext;
        private readonly IMapper _mapper;
        private readonly IUtils _utils;

        public AdmissionExamResultController(KalumDBContext kalumDBContext, IMapper mapper, IUtils utils)
        {
            this._kalumDBContext = kalumDBContext;
            this._mapper = mapper;
            this._utils = utils;
        }

        [HttpGet]
        public async Task<ActionResult<AdmissionExamResult>> Get()
        {
            long initialTime = DateTime.Now.Ticks;
            List<AdmissionExamResult> admissionExamResults = await this._kalumDBContext.AdmissionExamResults.ToListAsync();

            if (admissionExamResults == null ||
                admissionExamResults.Count == 0)
            {
                this._utils.Log(initialTime, ApiResponses.NoContent.Message, ApiResponses.NoContent.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.GET);
                return NoContent();
            }

            this._utils.Log(initialTime, ApiResponses.Success.Message, ApiResponses.Success.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.GET);
            return Ok(ApiResponseDTO<object>.Ok(this._mapper.Map<List<AdmissionExamResultListDTO>>(admissionExamResults)));
        }

        [HttpGet("{fileNumber}/{year}", Name = "GetAdmissionExamResultById")]
        public async Task<ActionResult> GetById(string fileNumber, string year)
        {
            long initialTime = DateTime.Now.Ticks;
            AdmissionExamResult admissionExamResult = await this._kalumDBContext.AdmissionExamResults.FirstOrDefaultAsync(r => r.FileNumber == fileNumber && r.Year == year);

            if (admissionExamResult == null)
            {
                this._utils.Log(initialTime, ApiResponses.NotFound.Message, ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.GET);
                return NotFound(ApiResponseDTO<object>.Fail("Error de búsqueda del resultado del examen.", new List<string> { $"No existe un resultado para el expediente {fileNumber} y año {year}" }));
            }
            this._utils.Log(initialTime, ApiResponses.Success.Message, ApiResponses.Success.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.GET);

            return Ok(ApiResponseDTO<object>.Ok(this._mapper.Map<AdmissionExamResultListDTO>(admissionExamResult)));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] AdmissionExamResultCreatedDTO admissionExamResultCreatedDTO)
        {
            long initialTime = DateTime.Now.Ticks;

            Applicant existingApplicant = await this._kalumDBContext.Applicants.FirstOrDefaultAsync(a => a.FileNumber == admissionExamResultCreatedDTO.FileNumber);

            if (existingApplicant == null)
            {
                this._utils.Log(initialTime, ApiResponses.BadRequest.Message, ApiResponses.BadRequest.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.POST);
                return BadRequest(ApiResponseDTO<object>.Fail("Crear resultado del examen.", new List<string> { $"No existe un expediente " + $"{admissionExamResultCreatedDTO.FileNumber} " + $"en el año {admissionExamResultCreatedDTO.Year} para poder ser registrado" }));
            }

            AdmissionExamResult existingResult = await this._kalumDBContext.AdmissionExamResults.FirstOrDefaultAsync(r => r.FileNumber == admissionExamResultCreatedDTO.FileNumber && r.Year == admissionExamResultCreatedDTO.Year);

            if (existingResult != null)
            {
                this._utils.Log(initialTime, ApiResponses.Conflict.Message, ApiResponses.Conflict.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.POST);
                return Conflict(ApiResponseDTO<object>.Fail("Crear resultado del examen.", new List<string> { $"Ya existe un resultado para el expediente " + $"{admissionExamResultCreatedDTO.FileNumber} " + $"en el año {admissionExamResultCreatedDTO.Year}" }));
            }

            AdmissionExamResult admissionExamResult = this._mapper.Map<AdmissionExamResult>(admissionExamResultCreatedDTO);
            await this._kalumDBContext.AdmissionExamResults.AddAsync(admissionExamResult);
            await this._kalumDBContext.SaveChangesAsync();

            this._utils.Log(initialTime, ApiResponses.Success.Message, ApiResponses.Success.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.POST);
            return CreatedAtRoute("GetAdmissionExamResultById", new { fileNumber = admissionExamResult.FileNumber, year = admissionExamResult.Year }, ApiResponseDTO<object>.Ok(this._mapper.Map<AdmissionExamResultListDTO>(admissionExamResult)));
        }

        [HttpPut("{fileNumber}/{year}")]
        public async Task<ActionResult> Update(string fileNumber, string year, [FromBody] AdmissionExamResultUpdateDTO admissionExamResultUpdateDTO)
        {
            long initialTime = DateTime.Now.Ticks;


            Applicant existingApplicant = await this._kalumDBContext.Applicants.FirstOrDefaultAsync(a => a.FileNumber == fileNumber);

            if (existingApplicant == null)
            {
                this._utils.Log(initialTime, ApiResponses.BadRequest.Message, ApiResponses.BadRequest.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.POST);
                return BadRequest(ApiResponseDTO<object>.Fail("Actualizar resultado del examen.", new List<string> { $"No existe un expediente " + $"{fileNumber} " + $"en el año {year} para poder ser actualizado" }));
            }

            AdmissionExamResult admissionExamResult = await this._kalumDBContext.AdmissionExamResults.FirstOrDefaultAsync(r => r.FileNumber == fileNumber && r.Year == year);

            if (admissionExamResult == null)
            {
                this._utils.Log(initialTime, ApiResponses.NotFound.Message, ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.PUT);
                return NotFound(ApiResponseDTO<object>.Fail("Actualizar resultado del examen.", new List<string> { $"No existe un resultado para el expediente {fileNumber} y año {year}" }));
            }

            this._mapper.Map(admissionExamResultUpdateDTO, admissionExamResult);
            await this._kalumDBContext.SaveChangesAsync();

            this._utils.Log(initialTime, ApiResponses.NoContent.Message, ApiResponses.NoContent.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.PUT);
            return NoContent();
        }

        [HttpDelete("{fileNumber}/{year}")]
        public async Task<ActionResult> Delete(string fileNumber,string year)
        {
            long initialTime = DateTime.Now.Ticks;
            AdmissionExamResult admissionExamResult = await this._kalumDBContext.AdmissionExamResults.FirstOrDefaultAsync(r => r.FileNumber == fileNumber && r.Year == year);

            if (admissionExamResult == null)
            {
                this._utils.Log(initialTime, ApiResponses.NotFound.Message,ApiResponses.NotFound.Code,Shared.Enums.TypeLog.ERROR,HttpContext,Shared.Enums.MethodLog.DELETE);
                return NotFound(ApiResponseDTO<object>.Fail("Eliminar resultado del examen.",new List<string>{$"No existe un resultado para el expediente {fileNumber} y año {year}"}));
            }

            this._kalumDBContext.AdmissionExamResults.Remove(admissionExamResult);
            await this._kalumDBContext.SaveChangesAsync();

            this._utils.Log(initialTime,ApiResponses.NoContent.Message,ApiResponses.NoContent.Code,Shared.Enums.TypeLog.INFORMATION,HttpContext,Shared.Enums.MethodLog.DELETE);
            return NoContent();
        }
    }
}
