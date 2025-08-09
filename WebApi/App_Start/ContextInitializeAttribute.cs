using Raven.Client;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;

namespace WebApi.App_Start
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ContextInitializeAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutedAsync(HttpActionExecutedContext ctx, CancellationToken ct)
        {
            {
                var container = GlobalConfiguration.Configuration.DependencyResolver;
                var method = ctx.Request.Method;
                if (method == HttpMethod.Post || method == HttpMethod.Put || method == HttpMethod.Delete)
                {
                    var session = (IAsyncDocumentSession)container.GetService(typeof(IAsyncDocumentSession));
                    await session.SaveChangesAsync();
                }
            }
        }
    }
}