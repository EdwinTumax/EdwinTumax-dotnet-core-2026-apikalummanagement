using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    [Route("kalum-management/v1/addmission-exam")]
    public class AdmissionExamController : ControllerBase
    {
        private readonly KalumDBContext _kalumDBContext;
        private readonly IMapper _mapper;
        private readonly IUtils _utils;
        public AdmissionExamController(KalumDBContext kalumDBContext, IMapper mapper, IUtils utils)
        {
            this._kalumDBContext = kalumDBContext;
            this._mapper = mapper;
            this._utils = utils;
        }

        [HttpGet]
        public async Task<ActionResult<AdmissionExam>> Get()
        {
            long initialTime = DateTime.Now.Ticks;
            List<AdmissionExam> exams = await this._kalumDBContext.AdmissionExams.ToListAsync();
            if (exams == null || exams.Count == 0)
            {
                this._utils.Log(initialTime, ApiResponses.NoContent.Message, ApiResponses.NoContent.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.GET);
                return NoContent();
            }
            this._utils.Log(initialTime, ApiResponses.Success.Message, ApiResponses.Success.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.GET);
            return Ok(ApiResponseDTO<Object>.Ok(this._mapper.Map<List<AdmissionExamListDTO>>(exams)));
        }

        [HttpGet("{examId}", Name = "GetAdmissionExamById")]
        public async Task<ActionResult<AdmissionExamListDTO>> GetById(string examId)
        {
            long initialTime = DateTime.Now.Ticks;
            AdmissionExam exam = await this._kalumDBContext.AdmissionExams.FirstOrDefaultAsync(e => e.ExamId == examId);
            if (exam == null)
            {
                this._utils.Log(initialTime, ApiResponses.NotFound.Message, ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.GET);
                return NotFound(ApiResponseDTO<Object>.Fail($"Error de búsqueda con examen {examId}", new List<string> { $"No existe el examen con el id {examId}" }));
            }

            this._utils.Log(initialTime, ApiResponses.Success.Message, ApiResponses.Success.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.GET);
            return Ok(ApiResponseDTO<Object>.Ok(this._mapper.Map<AdmissionExamListDTO>(exam)));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] AdmissionExamCreatedDTO admissionExamCreatedDTO)
        {
            long initialTime = DateTime.Now.Ticks;
            AdmissionExam exam = this._mapper.Map<AdmissionExam>(admissionExamCreatedDTO);
            exam.ExamId = Guid.NewGuid().ToString();
            await this._kalumDBContext.AdmissionExams.AddAsync(exam);
            await this._kalumDBContext.SaveChangesAsync();
            this._utils.Log(initialTime, ApiResponses.Success.Message, ApiResponses.Success.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.POST);
            return CreatedAtRoute("GetAdmissionExamById", new { examId = exam.ExamId }, ApiResponseDTO<Object>.Ok(this._mapper.Map<AdmissionExamListDTO>(exam)));
        }

        [HttpPut("{examId}")]
        public async Task<ActionResult> Update(string examId, [FromBody] AdmissionExamUpdateDTO admissionExamUpdateDTO)
        {
            long initialTime = DateTime.Now.Ticks;
            AdmissionExam exam = await this._kalumDBContext.AdmissionExams.FirstOrDefaultAsync(e => e.ExamId == examId);
            if (exam == null)
            {
                this._utils.Log(initialTime, ApiResponses.NotFound.Message, ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.PUT);
                return NotFound(ApiResponseDTO<Object>.Fail("Actualizar examen.", new List<string> { $"No existe examen con el id {examId}" }));
            }
            this._mapper.Map(admissionExamUpdateDTO, exam);
            await this._kalumDBContext.SaveChangesAsync();
            this._utils.Log(initialTime, ApiResponses.NoContent.Message, ApiResponses.NoContent.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.PUT);
            return NoContent();
        }

        [HttpDelete("{examId}")]
        public async Task<ActionResult> Delete(string examId)
        {
            long initialTime = DateTime.Now.Ticks;
            AdmissionExam exam = await this._kalumDBContext.AdmissionExams.FirstOrDefaultAsync(e => e.ExamId == examId);
            if (exam == null)
            {
                this._utils.Log(initialTime, ApiResponses.NotFound.Message, ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.DELETE);
                return NotFound(ApiResponseDTO<Object>.Fail($"Error al eliminar el examen {examId}", new List<string> { $"No existe el examen con el id {examId}" }));
            }
            this._kalumDBContext.AdmissionExams.Remove(exam);
            await this._kalumDBContext.SaveChangesAsync();
            this._utils.Log(initialTime, ApiResponses.NoContent.Message, ApiResponses.NoContent.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.DELETE);
            return NoContent();
        }

    }
}