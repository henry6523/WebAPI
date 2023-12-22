using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebAPI.Services
{
    public interface IResponseServiceRepository
    {
        IActionResult CustomNotFoundResponse(string message, object data = null);
        IActionResult CustomBadRequestResponse(string message, object data = null);
        IActionResult CustomOkResponse(string message, object data = null);
        IActionResult CustomCreatedResponse(string message, object data = null);
        IActionResult CustomNoContentResponse(string message, object data = null);
    }
}