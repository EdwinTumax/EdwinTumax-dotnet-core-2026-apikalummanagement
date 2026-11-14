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
    [Route("kalum-management/v1/charges")]
    public class ChargeController : ControllerBase
    {
        private readonly KalumDBContext _kalumDBContext;
        private readonly IMapper _mapper;
        private readonly IUtils _utils;

        public ChargeController(KalumDBContext kalumDBContext, IMapper mapper, IUtils utils)
        {
            this._kalumDBContext = kalumDBContext;
            this._mapper = mapper;
            this._utils = utils;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            long initialTime = DateTime.Now.Ticks;
            List<Charge> charges = await this._kalumDBContext.Charges.ToListAsync();

            if(charges == null || charges.Count == 0)
            {
                this._utils.Log(initialTime, ApiResponses.NoContent.Message, ApiResponses.NoContent.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.GET);
                return NoContent();
            }

            this._utils.Log(initialTime, ApiResponses.Success.Message, ApiResponses.Success.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.GET);
            return Ok(ApiResponseDTO<Object>.Ok(this._mapper.Map<List<ChargeListDTO>>(charges)));
        }

        [HttpGet("{chargeId}", Name = "GetChargeById")]
        public async Task<ActionResult> GetById(string chargeId)
        {
            long initialTime = DateTime.Now.Ticks;
            Charge charge = await this._kalumDBContext.Charges.FirstOrDefaultAsync(c => c.ChargeId == chargeId);

            if(charge == null)
            {
                this._utils.Log(initialTime, ApiResponses.NotFound.Message, ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.GET);
                return NotFound(ApiResponseDTO<Object>.Fail($"Error de búsqueda con cargo {chargeId}", new List<string>() {$"No existe el cargo con el id {chargeId}"}));
            }

            this._utils.Log(initialTime, ApiResponses.Success.Message, ApiResponses.Success.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.GET);
            return Ok(ApiResponseDTO<Object>.Ok(this._mapper.Map<ChargeListDTO>(charge)));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ChargeCreatedDTO chargeCreatedDTO)
        {
            long initialTime = DateTime.Now.Ticks;
            Charge existingCharge = await this._kalumDBContext.Charges.FirstOrDefaultAsync(c => c.Description == chargeCreatedDTO.Description);

            if(existingCharge != null)
            {
                this._utils.Log(initialTime, ApiResponses.Conflict.Message, ApiResponses.Conflict.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.POST);
                return Conflict(ApiResponseDTO<Object>.Fail("Crear cargo.", new List<string>() {$"Ya existe un cargo con la descripción {chargeCreatedDTO.Description}"}));
            }

            Charge charge = this._mapper.Map<Charge>(chargeCreatedDTO);
            charge.ChargeId = Guid.NewGuid().ToString();

            await this._kalumDBContext.Charges.AddAsync(charge);
            await this._kalumDBContext.SaveChangesAsync();

            this._utils.Log(initialTime, ApiResponses.Success.Message, ApiResponses.Success.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.POST);
            return CreatedAtRoute("GetChargeById", new { chargeId = charge.ChargeId }, ApiResponseDTO<Object>.Ok(this._mapper.Map<ChargeListDTO>(charge)));
        }

        [HttpPut("{chargeId}")]
        public async Task<ActionResult> Update(string chargeId, [FromBody] ChargeUpdateDTO chargeUpdateDTO)
        {
            long initialTime = DateTime.Now.Ticks;
            Charge charge = await this._kalumDBContext.Charges.FirstOrDefaultAsync(c => c.ChargeId == chargeId);

            if(charge == null)
            {
                this._utils.Log(initialTime, ApiResponses.NotFound.Message, ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.PUT);
                return NotFound(ApiResponseDTO<Object>.Fail("Actualizar cargo.", new List<string>() {$"No existe el cargo con el id {chargeId}"}));
            }

            this._mapper.Map(chargeUpdateDTO, charge);
            await this._kalumDBContext.SaveChangesAsync();

            this._utils.Log(initialTime, ApiResponses.NoContent.Message, ApiResponses.NoContent.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.PUT);
            return NoContent();
        }

        [HttpDelete("{chargeId}")]
        public async Task<ActionResult> Delete(string chargeId)
        {
            long initialTime = DateTime.Now.Ticks;
            Charge charge = await this._kalumDBContext.Charges.FirstOrDefaultAsync(c => c.ChargeId == chargeId);

            if(charge == null)
            {
                this._utils.Log(initialTime, ApiResponses.NotFound.Message, ApiResponses.NotFound.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.DELETE);
                return NotFound(ApiResponseDTO<Object>.Fail($"Error al eliminar el cargo {chargeId}", new List<string>() {$"No existe el cargo con el id {chargeId}"}));
            }

            this._kalumDBContext.Charges.Remove(charge);
            await this._kalumDBContext.SaveChangesAsync();

            this._utils.Log(initialTime, ApiResponses.NoContent.Message, ApiResponses.NoContent.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.DELETE);
            return NoContent();
        }
    }
}