using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters; // Fix: Use correct namespace for ActionFilterAttribute and ActionExecutingContext

namespace myApi.CustomAcionFilters
{
    public class ValidateModelAttribute : ActionFilterAttribute // Fix: Correct class name casing
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid) // Fix: Use 'context' instead of 'filterContext'
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
            base.OnActionExecuting(context);
        }
    }
}
