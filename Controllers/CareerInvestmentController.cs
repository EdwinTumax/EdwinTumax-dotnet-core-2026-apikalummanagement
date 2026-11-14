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
    [Route("kalum-management/v1/career-investments")]
    public class CareerInvestmentController : ControllerBase
    {
        private readonly KalumDBContext _kalumDBContext;
        private readonly IMapper _mapper;
        private readonly IUtils _utils;
        public CareerInvestmentController(KalumDBContext kalumDBContext, IMapper mapper, IUtils utils)
        {
            this._kalumDBContext = kalumDBContext;
            this._mapper = mapper;
            this._utils = utils;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            long initialTime = DateTime.Now.Ticks;
            List<CareerInvestment> careerInvestments = await this._kalumDBContext.CareerInvestments.ToListAsync();

            if (careerInvestments == null || careerInvestments.Count == 0)
            {
                this._utils.Log(initialTime, ApiResponses.NoContent.Message, ApiResponses.NoContent.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.GET);
                return NoContent();
            }

            this._utils.Log(initialTime, ApiResponses.Success.Message, ApiResponses.Success.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.GET);
            return Ok(ApiResponseDTO<Object>.Ok(this._mapper.Map<List<CareerInvestmentListDTO>>(careerInvestments)));
        }

        [HttpGet("{investmentId}", Name = "GetCareerInvestmentById")]
        public async Task<ActionResult> GetById(string investmentId)
        {
            long initialTime = DateTime.Now.Ticks;
            CareerInvestment careerInvestment = await this._kalumDBContext.CareerInvestments.FirstOrDefaultAsync(c => c.InvestmentId == investmentId);

            if (careerInvestment == null)
            {
                this._utils.Log(initialTime, ApiResponses.NotFound.Message, ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.GET);
                return NotFound(ApiResponseDTO<Object>.Fail($"Error de búsqueda con inversión {investmentId}", new List<string>() { $"No existe la inversión con el id {investmentId}" }));
            }

            this._utils.Log(initialTime, ApiResponses.Success.Message, ApiResponses.Success.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.GET);
            return Ok(ApiResponseDTO<Object>.Ok(this._mapper.Map<CareerInvestmentListDTO>(careerInvestment)));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CareerInvestmentCreatedDTO careerInvestmentCreatedDTO)
        {
            long initialTime = DateTime.Now.Ticks;
         
            CareerInvestment existingCareerInvestment = await this._kalumDBContext.CareerInvestments.FirstOrDefaultAsync(c => c.CareerId == careerInvestmentCreatedDTO.CareerId);
            if(existingCareerInvestment != null)
            {
                this._utils.Log(initialTime, ApiResponses.Conflict.Message, ApiResponses.Conflict.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.POST);
                return Conflict(ApiResponseDTO<Object>.Fail("Crear inversión de carrera.", new List<string>() {$"Ya existe una inversión de la carrera con el id {careerInvestmentCreatedDTO.CareerId}"}));
            }

            TechnicalCareer existingTechnicalCareer = await this._kalumDBContext.TechnicalCareers.FirstOrDefaultAsync(c => c.CareerId == careerInvestmentCreatedDTO.CareerId);
            if (existingTechnicalCareer == null)
            {
                this._utils.Log(initialTime, ApiResponses.NotFound.Message, ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.POST);
                return NotFound(ApiResponseDTO<Object>.Fail($"Error al crear la inversión de la carrera con el id {careerInvestmentCreatedDTO.CareerId}", new List<string>() { $"No existe la carrera técnica con el id {careerInvestmentCreatedDTO.CareerId}" }));
            }

            CareerInvestment careerInvestment = this._mapper.Map<CareerInvestment>(careerInvestmentCreatedDTO);
            careerInvestment.InvestmentId = Guid.NewGuid().ToString();
            await this._kalumDBContext.CareerInvestments.AddAsync(careerInvestment);
            await this._kalumDBContext.SaveChangesAsync();

            this._utils.Log(initialTime, ApiResponses.Success.Message, ApiResponses.Success.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.POST);
            return CreatedAtRoute("GetCareerInvestmentById", new { investmentId = careerInvestment.InvestmentId }, ApiResponseDTO<Object>.Ok(this._mapper.Map<CareerInvestmentListDTO>(careerInvestment)));
        }

        [HttpPut("{investmentId}")]
        public async Task<ActionResult> Update(string investmentId, [FromBody] CareerInvestmentUpdateDTO careerInvestmentUpdateDTO)
        {
            long initialTime = DateTime.Now.Ticks;

            CareerInvestment careerInvestment = await this._kalumDBContext.CareerInvestments.FirstOrDefaultAsync(c => c.InvestmentId == investmentId);
            if (careerInvestment == null)
            {
                this._utils.Log(initialTime, ApiResponses.NotFound.Message, ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.PUT);
                return NotFound(ApiResponseDTO<Object>.Fail("Actualizar inversión de carrera.", new List<string>() { $"No existe la inversión con el id {investmentId}" }));
            }

            TechnicalCareer existingTechnicalCareer = await this._kalumDBContext.TechnicalCareers.FirstOrDefaultAsync(c => c.CareerId == careerInvestmentUpdateDTO.CareerId);
            if (existingTechnicalCareer == null)
            {
                this._utils.Log(initialTime, ApiResponses.NotFound.Message, ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.POST);
                return NotFound(ApiResponseDTO<Object>.Fail($"Error al actualizar la inversión de la carrera con el id {careerInvestmentUpdateDTO.CareerId}", new List<string>() { $"No existe la carrera técnica con el id {careerInvestmentUpdateDTO.CareerId}" }));
            }

            this._mapper.Map(careerInvestmentUpdateDTO, careerInvestment);
            await this._kalumDBContext.SaveChangesAsync();

            this._utils.Log(initialTime, ApiResponses.NoContent.Message, ApiResponses.NoContent.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.PUT);
            return NoContent();
        }

        [HttpDelete("{investmentId}")]
        public async Task<ActionResult> Delete(string investmentId)
        {
            long initialTime = DateTime.Now.Ticks;
            CareerInvestment careerInvestment = await this._kalumDBContext.CareerInvestments.FirstOrDefaultAsync(c => c.InvestmentId == investmentId);

            if (careerInvestment == null)
            {
                this._utils.Log(initialTime, ApiResponses.NotFound.Message, ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.DELETE);
                return NotFound(ApiResponseDTO<Object>.Fail($"Error al eliminar la inversión {investmentId}", new List<string>() { $"No existe la inversión con el id {investmentId}" }));
            }

            this._kalumDBContext.CareerInvestments.Remove(careerInvestment);
            await this._kalumDBContext.SaveChangesAsync();

            this._utils.Log(initialTime, ApiResponses.NoContent.Message, ApiResponses.NoContent.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.DELETE);
            return NoContent();
        }
    }
}