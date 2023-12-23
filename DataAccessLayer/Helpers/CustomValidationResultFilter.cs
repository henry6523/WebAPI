using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Helpers
{
    public class CustomValidationResultFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult objectResult && objectResult.Value is ValidationProblemDetails validationProblem)
            {
                var formattedErrors = MapToYourPreferredFormat(validationProblem);

                var newObjectResult = new ObjectResult(formattedErrors)
                {
                    StatusCode = (int)validationProblem.Status,
                };

                context.Result = newObjectResult;
            }
        }

        private object MapToYourPreferredFormat(ValidationProblemDetails validationProblem)
        {
            var formattedErrors = new
            {
                Status = (int)validationProblem.Status,
                errors = validationProblem.Errors,
            };

            return formattedErrors;
        }
    }
}
