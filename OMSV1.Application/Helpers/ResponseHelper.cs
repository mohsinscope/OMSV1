using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace OMSV1.Application.Helpers;

    public static class ResponseHelper
    {
        public static IActionResult CreateErrorResponse(HttpStatusCode statusCode, string message, IEnumerable<string> errors = null)
        {
            var errorResponse = new
            {
                Message = message,
                Errors = errors ?? new List<string>()
            };

            return new ObjectResult(errorResponse)
            {
                StatusCode = (int)statusCode
            };
        }
    }



