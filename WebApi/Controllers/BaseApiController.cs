// Cleanup notes:
// 1. Method names like Found() were misleading, implying GET so I renamed for clarity
// 2. Record creation endpoints now use CreatedAtRoute to return 201 and Location header
// 3. I switched from HttpResponseMessage to IHttpActionResult for cleaner, testable code
// 4. All helpers are protected to avoid appearing as public endpoints
// 5. Removed redundant ControllerContext usage
// 6. Switched methods to lambda versions for simplicity
// 7. Renamed methods for clarity
// 8. Added helper method for delete endpoints
// 9. Added extra helper method for internal server errors

using System.Net;
using System.Web.Http;

namespace WebApi.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        protected IHttpActionResult Created(object body, object routeValues, string routeName)
        => CreatedAtRoute(routeName, routeValues, body);

        protected IHttpActionResult OkResponse(object body) => Ok(body);

        protected IHttpActionResult NotFoundResponse() => NotFound();

        protected IHttpActionResult NoContentResponse() => StatusCode(HttpStatusCode.NoContent);
        
        protected IHttpActionResult InternalServerErrorResponse() => StatusCode(HttpStatusCode.InternalServerError);

    }
}