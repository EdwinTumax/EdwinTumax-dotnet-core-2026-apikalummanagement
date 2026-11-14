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
    [Route("kalum-management/v1/technical-careers")]
    public class TechnicalCareerController : ControllerBase
    {
        private readonly KalumDBContext _kalumDBContext;
        private readonly IMapper _mapper;
        private readonly IUtils _utils;

        public TechnicalCareerController(KalumDBContext kalumDBContext, IMapper mapper, IUtils utils)
        {
            this._kalumDBContext = kalumDBContext;
            this._mapper = mapper;
            this._utils = utils;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            long initialTime = DateTime.Now.Ticks;
            List<TechnicalCareer> technicalCareers = await this._kalumDBContext.TechnicalCareers.ToListAsync();

            if(technicalCareers == null || technicalCareers.Count == 0)
            {
                this._utils.Log(initialTime, ApiResponses.NoContent.Message, ApiResponses.NoContent.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.GET);
                return NoContent();
            }

            this._utils.Log(initialTime, ApiResponses.Success.Message, ApiResponses.Success.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.GET);
            return Ok(ApiResponseDTO<Object>.Ok(this._mapper.Map<List<TechnicalCareerListDTO>>(technicalCareers)));
        }

        [HttpGet("{careerId}", Name = "GetTechnicalCareerById")]
        public async Task<ActionResult> GetById(string careerId)
        {
            long initialTime = DateTime.Now.Ticks;
            TechnicalCareer technicalCareer = await this._kalumDBContext.TechnicalCareers.FirstOrDefaultAsync(c => c.CareerId == careerId);

            if(technicalCareer == null)
            {
                this._utils.Log(initialTime, ApiResponses.NotFound.Message, ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.GET);
                return NotFound(ApiResponseDTO<Object>.Fail($"Error de búsqueda con carrera técnica {careerId}", new List<string>() {$"No existe la carrera técnica con el id {careerId}"}));
            }

            this._utils.Log(initialTime, ApiResponses.Success.Message, ApiResponses.Success.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.GET);
            return Ok(ApiResponseDTO<Object>.Ok(this._mapper.Map<TechnicalCareerListDTO>(technicalCareer)));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TechnicalCareerCreatedDTO technicalCareerCreatedDTO)
        {
            long initialTime = DateTime.Now.Ticks;
            TechnicalCareer existingTechnicalCareer = await this._kalumDBContext.TechnicalCareers.FirstOrDefaultAsync(c => c.Name == technicalCareerCreatedDTO.Name);

            if(existingTechnicalCareer != null)
            {
                this._utils.Log(initialTime, ApiResponses.Conflict.Message, ApiResponses.Conflict.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.POST);
                return Conflict(ApiResponseDTO<Object>.Fail("Crear carrera técnica.", new List<string>() {$"Ya existe una carrera técnica con el nombre {technicalCareerCreatedDTO.Name}"}));
            }

            TechnicalCareer technicalCareer = this._mapper.Map<TechnicalCareer>(technicalCareerCreatedDTO);
            technicalCareer.CareerId = Guid.NewGuid().ToString();

            await this._kalumDBContext.TechnicalCareers.AddAsync(technicalCareer);
            await this._kalumDBContext.SaveChangesAsync();

            this._utils.Log(initialTime, ApiResponses.Success.Message, ApiResponses.Success.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.POST);
            return CreatedAtRoute("GetTechnicalCareerById", new { careerId = technicalCareer.CareerId }, ApiResponseDTO<Object>.Ok(this._mapper.Map<TechnicalCareerListDTO>(technicalCareer)));
        }

        [HttpPut("{careerId}")]
        public async Task<ActionResult> Update(string careerId, [FromBody] TechnicalCareerUpdateDTO technicalCareerUpdateDTO)
        {
            long initialTime = DateTime.Now.Ticks;
            TechnicalCareer technicalCareer = await this._kalumDBContext.TechnicalCareers.FirstOrDefaultAsync(c => c.CareerId == careerId);

            if(technicalCareer == null)
            {
                this._utils.Log(initialTime, ApiResponses.NotFound.Message, ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.PUT);
                return NotFound(ApiResponseDTO<Object>.Fail("Actualizar carrera técnica.", new List<string>() {$"No existe la carrera técnica con el id {careerId}"}));
            }

            this._mapper.Map(technicalCareerUpdateDTO, technicalCareer);
            await this._kalumDBContext.SaveChangesAsync();

            this._utils.Log(initialTime, ApiResponses.NoContent.Message, ApiResponses.NoContent.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.PUT);
            return NoContent();
        }

        [HttpDelete("{careerId}")]
        public async Task<ActionResult> Delete(string careerId)
        {
            long initialTime = DateTime.Now.Ticks;
            TechnicalCareer technicalCareer = await this._kalumDBContext.TechnicalCareers.FirstOrDefaultAsync(c => c.CareerId == careerId);

            if(technicalCareer == null)
            {
                this._utils.Log(initialTime, ApiResponses.NotFound.Message, ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.DELETE);
                return NotFound(ApiResponseDTO<Object>.Fail($"Error al eliminar la carrera técnica {careerId}", new List<string>() {$"No existe la carrera técnica con el id {careerId}"}));
            }

            this._kalumDBContext.TechnicalCareers.Remove(technicalCareer);
            await this._kalumDBContext.SaveChangesAsync();

            this._utils.Log(initialTime, ApiResponses.NoContent.Message, ApiResponses.NoContent.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.DELETE);
            return NoContent();
        }
    }
}