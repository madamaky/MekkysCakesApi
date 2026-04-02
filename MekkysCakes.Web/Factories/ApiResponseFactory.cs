using Microsoft.AspNetCore.Mvc;

namespace MekkysCakes.Web.Factories
{
    public static class ApiResponseFactory
    {
        public static IActionResult GenerateApiValidationResponse(ActionContext actionContext)
        {
            var erros = actionContext.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .ToDictionary(
                    x => x.Key,
                    x => x.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
                );

            var problem = new ProblemDetails()
            {
                Title = "Validation error while processing the HTTP request",
                Detail = "One or more validation errors occurred while processing the HTTP request",
                Status = StatusCodes.Status400BadRequest,
                Extensions = { { "Errors", erros } }
            };

            return new BadRequestObjectResult(problem);
        }
    }
}
