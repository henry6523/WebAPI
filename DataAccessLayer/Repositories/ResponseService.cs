using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer.DTO;
using DataAccessLayer.Data;
using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyWebAPI.Services
{
    public class ResponseService : IResponseService
    {
        public IActionResult CustomNotFoundResponse(string message, object data = null)
        {
            var responseObj = new
            {
                status = 404,
                message = message,
                data = data
            };
            return new NotFoundObjectResult(responseObj);
        }

        public IActionResult CustomBadRequestResponse(string message, object data = null)
        {
            var responseObj = new
            {
                status = 400,
                message = message,
                data = data
            };
            return new BadRequestObjectResult(responseObj);
        }

        public IActionResult CustomOkResponse(string message, object data = null)
        {
            var successResponse = new
            {
                status = 200,
                message = message,
                data = data
            };
            return new OkObjectResult(successResponse);
        }

        public IActionResult CustomCreatedResponse(string message, object data = null)
        {
            var responseObj = new
            {
                status = 201,
                message = message,
                data = data
            };
            return new ObjectResult(responseObj) { StatusCode = 201 };

        }
    }
}