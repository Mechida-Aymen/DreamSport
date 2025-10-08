using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace gestionEmployer.API.Filters
{
    public class ValidationModels : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                // Return les detailles d'erreur de validation
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}
