using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiKalumManagement.Shared.Constants
{
    public static class ApiResponses
    {
        public static readonly (int Code, string Message) Success = (200, "CONSULTA REALIZADA CON ÉXITO");
        public static readonly (int Code, string Message) Created = (201, "RECURSO CREADO DE FORMA EXITOSA");
        public static readonly (int Code, string Message) Accepted = (202, "EL SERVICIO ACEPTO SU SOLICITUD DE FORMA EXITOSA");
        public static readonly (int Code, string Message) NoContent = (204, "LA CONSULTA SE REALIZO CON ÉXITO PERO NO SE ENCONTRARON DATOS");
        public static readonly (int Code, string Message) BadRequest = (400, "PETICION INVALIDA");
        public static readonly (int Code, string Message) Unauthorized = (401, "SIN AUTORIZACIÓN");
        public static readonly (int Code, string Message) Forbidden = (403, "ACCESO DENEGADO");
        public static readonly (int Code, string Message) NotFound = (404, "RECURSO NO ENCONTRADO");
        public static readonly (int Code, string Message) Conflict = (409, "EL RECURSO EN REALIDAD EXISTE");
        public static readonly (int Code, string Message) InternalServerError = (500, "ERROR INTERNO DEL SERVICIO");
        public static readonly (int Code, string Message) ServiceUnavailable = (503, "ERROR INTERNO DEL SERVICIO");
        public static readonly (int Code, string Message) GatewayTimeout = (504, "TIEMPO DE RESPOUESTA AGOTADO");
    }
}