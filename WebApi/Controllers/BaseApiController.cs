// Cleanup notes:
// 1. Removed all the redundant code. IHttpActionResult response messages simpler
// 1. Added helper method for delete endpoints

using System.Net;
using System.Web.Http;

namespace WebApi.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        protected IHttpActionResult NoContent() => StatusCode(HttpStatusCode.NoContent);   
    }
}