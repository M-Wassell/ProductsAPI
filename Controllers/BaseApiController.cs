using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ProductsAPI.Controllers
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        // Converts FluentValidation results to  ValidationProblem (RFC 7807) !! Instead of empty string in Swgger response
        // Makes the Product Controller DRY 
        protected ActionResult ValidationProblem(ValidationResult result)
        {
            var modelStateDictionary = new ModelStateDictionary();

            foreach (var error in result.Errors)
            {
                var key = string.IsNullOrEmpty(error.PropertyName) ? "CustomError" : error.PropertyName;
                
                modelStateDictionary.AddModelError(key, error.ErrorMessage);
            }

            return ValidationProblem(modelStateDictionary);
        }
    }
}
