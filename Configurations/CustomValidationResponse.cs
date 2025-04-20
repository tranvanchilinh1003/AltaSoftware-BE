using ISC_ELIB_SERVER.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Configurations
{
    public class CustomValidationResponse
    {
        public static IActionResult GenerateResponse(ActionContext context)
        {
            var errors = context.ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .ToDictionary(
                    e => e.Key,
                    e => e.Value.Errors.Select(err => err.ErrorMessage).ToArray()
                );

            return new BadRequestObjectResult(ApiResponse<Dictionary<string, string[]>>.Error(errors));
        }
    }
}
