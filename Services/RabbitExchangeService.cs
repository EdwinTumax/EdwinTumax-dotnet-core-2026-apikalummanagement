using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ApiKalumManagement.DTOs.Requests;
using ApiKalumManagement.Services.Interfaces;
using RabbitMQ.Client;

namespace ApiKalumManagement.Services
{
    public class RabbitExchangeService : IRabbitExchangeService
    {

        private readonly ILogger<RabbitExchangeService> _logger;
        private readonly IConfiguration _configuration;

        public RabbitExchangeService(ILogger<RabbitExchangeService> logger, IConfiguration configuration)
        {
            this._configuration = configuration;
            this._logger = logger;
        }        

        public async Task<bool> ApplicantCreatedOrderAsync(ApplicantCreateOrderDTO order)
        {
               bool response = false;
               ConnectionFactory connectionFactory = new ConnectionFactory();
               IConnection connection = null;
               IChannel channel = null;
               connectionFactory.HostName = this._configuration.GetValue<string>("ConnectionsRabbit:Host");
               connectionFactory.VirtualHost = this._configuration.GetValue<string>("ConnectionsRabbit:VirtualHost"); 
               connectionFactory.Port = this._configuration.GetValue<int>("ConnectionsRabbit:Port");
               connectionFactory.UserName = this._configuration.GetValue<string>("ConnectionsRabbit:User");
               connectionFactory.Password = this._configuration.GetValue<string>("ConnectionsRabbit:Password");
               connection = await connectionFactory.CreateConnectionAsync();
               channel = await connection.CreateChannelAsync();
               await channel.BasicPublishAsync(this._configuration.GetValue<string>("ConnectionsRabbit:Exchange"),this._configuration.GetValue<string>("ConnectionsRabbit:RouteKey"),false,Encoding.UTF8.GetBytes(JsonSerializer.Serialize(order)));
               response = true; 
               await Task.Delay(this._configuration.GetValue<int>("ConnectionsRabbit:Delay")); 
               await channel.CloseAsync();
               await connection.CloseAsync();
               return response;
        }
    }
}