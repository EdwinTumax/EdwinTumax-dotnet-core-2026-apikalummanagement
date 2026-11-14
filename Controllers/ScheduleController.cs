using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiKalumManagement.DBContexts;
using ApiKalumManagement.DTOs.Common;
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
    [Route("kalum-management/v1/schedules")]
    public class ScheduleController : ControllerBase
    {
        private readonly KalumDBContext _kalumDBContext;
        private readonly IMapper _mapper;
        private readonly IUtils _utils;

        public ScheduleController(KalumDBContext kalumDBContext, IMapper mapper, IUtils utils)
        {
            this._kalumDBContext = kalumDBContext;
            this._mapper = mapper;
            this._utils = utils;

        }

        [HttpGet]
        public async Task<ActionResult<Schedule>> Get()
        {

            long initialTime = DateTime.Now.Ticks;
            List<Schedule> schedules = await this._kalumDBContext.Schedules.ToListAsync();
            if (schedules == null || schedules.Count == 0)
            {
                this._utils.Log(initialTime, ApiResponses.NoContent.Message, ApiResponses.NoContent.Code, Shared.Enums.TypeLog.ERROR, HttpContext, Shared.Enums.MethodLog.GET);
                return NoContent();
            }
            this._utils.Log(initialTime, ApiResponses.Success.Message, ApiResponses.Success.Code, Shared.Enums.TypeLog.INFORMATION, HttpContext, Shared.Enums.MethodLog.GET);
            return Ok(ApiResponseDTO<Object>.Ok(this._mapper.Map<List<ScheduleListDTO>>(schedules)));
        }
        
    }
}