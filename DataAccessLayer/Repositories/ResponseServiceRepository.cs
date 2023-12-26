using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ModelsLayer.DTO;
using DataAccessLayer.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace DataAccessLayer.Repositories
{
    public class ResponseServiceRepository : IResponseServiceRepository
    {
        public IActionResult CustomNotFoundResponse(string message, object data = null)
        {
            var responseObj = new
            {
                status = 404,
                message,
                data
            };
            return new NotFoundObjectResult(responseObj);
        }

        public IActionResult CustomOkResponse(string message, object data = null)
        {
            var successResponse = new
            {
                status = 200,
                message,
                data
            };
            return new OkObjectResult(successResponse);
        }

        public IActionResult CustomCreatedResponse(string message, object data = null)
        {
            var responseObj = new
            {
                status = 201,
                message,
                data
            };
            return new ObjectResult(responseObj) { StatusCode = 201 };

        }

        public IActionResult CustomBadRequestResponse(string message, object data = null)
        {
            var responseObj = new
            {
                status = 400,
                message,
                data
            };
            return new BadRequestObjectResult(responseObj);
        }

        public IActionResult CustomNotImplementedResponse(string message, object data = null)
        {
            var responseObj = new
            {
                status = 501,
                message,
                data
            };
            return new StatusCodeResult(StatusCodes.Status501NotImplemented);
        }

    }
}