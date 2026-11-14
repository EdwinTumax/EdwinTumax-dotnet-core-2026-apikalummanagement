using System.Text.Json;
using ApiKalumManagement.DTOs.Common;
using ApiKalumManagement.Services.Interfaces;
using ApiKalumManagement.Shared.Enums;

namespace ApiKalumManagement.Shared.Helpers
{
    public class Utils : IUtils
    {
        private readonly ILogger<Utils> _logger;

        private readonly IConfiguration _configuration;

        public Utils(ILogger<Utils> logger, IConfiguration configuration)
        {
            this._logger = logger;
            this._configuration = configuration;
        }

        public void Log(long initialTime, string message, int responseCode, TypeLog typeLog, HttpContext httpContext, MethodLog methodLog)
        {
            Console.WriteLine("IMPRESION DE LOG");
            LogDTO log = new LogDTO();
            log.Name = this._configuration.GetValue<string>("LocalServer:name");
            log.HostName = httpContext.Request.Host.Value;
            httpContext.Request.Headers.TryGetValue("Authorization", out var apikey);
            log.ApiKey = String.IsNullOrEmpty(apikey) ? "" : apikey.ToString().Split(" ")[1].Split(".")[1];
            log.Uri = httpContext.Request.Path;
            log.ResponseCode = responseCode;
            log.ResponseTime = DateTime.Now.Ticks - initialTime;
            log.ClientIp = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? httpContext.Connection.RemoteIpAddress.ToString();
            log.Pid = "1";
            log.Method = methodLog.ToString();
            log.Message = message;
            log.DateTime = GetDateWithFormat(DateTime.Now);
            log.Version = 1;
            switch (typeLog)
            {
                case TypeLog.DEBUG:
                    log.Level = 10;
                    this._logger.LogDebug(JsonSerializer.Serialize(log));
                    break;
                case TypeLog.INFORMATION:
                    Console.WriteLine("INFORMATION");
                    log.Level = 20;
                    this._logger.LogInformation(JsonSerializer.Serialize(log));
                    break;
                case TypeLog.WARNING:
                    log.Level = 30;
                    this._logger.LogWarning(JsonSerializer.Serialize(log));
                    break;
                case TypeLog.ERROR:
                    log.Level = 40;
                    this._logger.LogError(JsonSerializer.Serialize(log));
                    break;
                case TypeLog.CRITICAL:
                    log.Level = 50;
                    this._logger.LogCritical(JsonSerializer.Serialize(log));
                    break;
                default:
                    log.Level = 10;
                    this._logger.LogDebug(JsonSerializer.Serialize(log));
                    break;
            }
        }

        public string GetDateWithFormat(DateTime dateTime)
        {
            return dateTime.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
        }

    }
}