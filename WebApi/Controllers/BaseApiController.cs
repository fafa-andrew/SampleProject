// Cleanup notes:
// 1. Method names like Found() were misleading, implying GET so I renamed for clarity
// 2. Record creation endpoints now use CreatedAtRoute to return 201 and Location header
// 3. I switched from HttpResponseMessage to IHttpActionResult for cleaner, testable code
// 4. All helpers are protected to avoid appearing as public endpoints
// 5. Removed redundant ControllerContext usage
// 6. Switched methods to lambda versions for simplicity
// 7. Renamed methods for clarity
// 8. Added helper method for delete endpoints

using System.Net;
using System.Web.Http;

namespace WebApi.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        protected IHttpActionResult CreatedAt(object body, object routeValues, string routeName)
        => CreatedAtRoute(routeName, routeValues, body);

        protected IHttpActionResult OkRequest(object body) => Ok(body);

        protected IHttpActionResult RecordNotFound() => NotFound();

        protected IHttpActionResult NoContentResponse() => StatusCode(HttpStatusCode.NoContent);

    }
}