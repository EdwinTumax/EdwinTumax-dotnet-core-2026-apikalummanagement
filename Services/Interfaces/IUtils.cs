using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiKalumManagement.Shared.Enums;

namespace ApiKalumManagement.Services.Interfaces
{
    public interface IUtils
    {
        public void Log(long initialTime, string message, int responseCode, TypeLog typeLog, HttpContext httpContext, MethodLog methodLog);
    }
}