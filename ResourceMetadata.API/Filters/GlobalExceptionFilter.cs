using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace ResourceMetadata.API.Filters
{
    /// <summary>
    /// global exception hanlder / filter
    /// </summary>
    public class GlobalExceptionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var modelState = actionContext.ModelState;

            //this part is to cater for a unique situation where the client send an empty query parameter e.g http://localhost/api/get?param=
            //and the coressponding parameter in the api action is an optional parameter
            //default model binder has trouble to determine the 'empty value' stands for null or '' in some cases          
            //so we will skip the validation on any optional parameters(since they have an default value anyway) that passed as 'empty value'
            var parameters = actionContext.ActionDescriptor.GetParameters().Where(x=>x.IsOptional).Select(x=>x.ParameterName).ToList();
            foreach (var state in modelState)
            {
                if (state.Value.Errors.Count == 0)
                    continue;

                if ((parameters.Contains(state.Key) || parameters.Contains(state.Key.Split('.').First())))
                {
                    foreach (var error in state.Value.Errors.ToArray())
                    {
                        //only clear error thats caused by 'empty query string'
                        //this is the default [Required] error message
                        if (string.Compare(error.ErrorMessage, "A value is required but was not present in the request.", StringComparison.OrdinalIgnoreCase) == 0 &&
                            error.Exception == null)
                        {
                            state.Value.Errors.Remove(error);
                        }
                    }
                }
            }

            if (!modelState.IsValid)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, modelState);
                return;
            }

            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is DbEntityValidationException)
            {
                var modelState = actionExecutedContext.ActionContext.ModelState;
                var exception = actionExecutedContext.Exception as DbEntityValidationException;
                if (exception.EntityValidationErrors.Any())
                {
                    exception.EntityValidationErrors.First()
                        .ValidationErrors.AsParallel()
                        .ForAll(x => modelState.AddModelError(x.PropertyName, x.ErrorMessage));
                }

                actionExecutedContext.Response =
                    actionExecutedContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, modelState);
            }
            else
            {
                base.OnActionExecuted(actionExecutedContext);
            }
        }
    }
}
