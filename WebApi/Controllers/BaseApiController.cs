// Cleanup notes:
// 1. Removed all the redundant code. IHttpActionResult response messages simpler

using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace WebApi.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        protected IHttpActionResult BadRequestResponse(ModelStateDictionary modelState)
        {
            var errorMessage = string.Join("; ",
                modelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => string.IsNullOrEmpty(e.ErrorMessage)
                                 ? e.Exception?.Message
                                 : e.ErrorMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m)));

            return Content(HttpStatusCode.BadRequest, new { error = errorMessage });
        }

        protected IHttpActionResult NoContentResponse() => StatusCode(HttpStatusCode.NoContent);

        protected IHttpActionResult RecordExistsResponse() 
            => Content(HttpStatusCode.Conflict, new { error = "Resource already exists" });

        protected IHttpActionResult ResourceNotFoundResponse()
            => Content(HttpStatusCode.NotFound, new { error = "Resource not found" });

        protected IHttpActionResult InternalServerErrorResponse() 
            => Content(HttpStatusCode.InternalServerError, new { error = "An internal error occurred. We've been notified!" });
    }
}