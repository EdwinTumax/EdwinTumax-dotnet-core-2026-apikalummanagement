using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiKalumManagement.DTOs.Requests;

namespace ApiKalumManagement.Services.Interfaces
{
    public interface IRabbitExchangeService
    {
        public Task<bool> ApplicantCreatedOrderAsync(ApplicantCreateOrderDTO order);
    }
}