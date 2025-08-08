// Cleanup notes:
// 1. All endpoints now return IHttpActionResult for testability purposes
// 2. I ensured that all methods return the appropriate status codes for REST compatibility
// 3. Re-arranged methods in GET, POST, PUT, Delete order for readability
// 4. Changed the HTTP verb for /update from POST to PUT
// 5. Removed the route attribute for /create and /update endpoints because the userId param is not needed
// 6. Removed the 'user' suffix from method names because its redundant. We are already in the users controller so no need

using System;
using System.Linq;
using System.Web.Http;
using BusinessEntities;
using Core.Services.Users;
using WebApi.Models.Users;

namespace WebApi.Controllers
{
    [RoutePrefix("users")]
    public class UserController : BaseApiController
    {
        private readonly ICreateUserService _createUserService;
        private readonly IDeleteUserService _deleteUserService;
        private readonly IGetUserService _getUserService;
        private readonly IUpdateUserService _updateUserService;

        public UserController(
            ICreateUserService createUserService, 
            IDeleteUserService deleteUserService, 
            IGetUserService getUserService, 
            IUpdateUserService updateUserService
            )
        {
            _createUserService = createUserService;
            _deleteUserService = deleteUserService;
            _getUserService = getUserService;
            _updateUserService = updateUserService;
        }

        [Route("list")]
        [HttpGet]
        public IHttpActionResult Get(int skip, int take, UserTypes? type = null, string name = null, string email = null)
        {
            var users = _getUserService.GetUsers(type, name, email)
                                       .Skip(skip).Take(take)
                                       .Select(q => new UserData(q))
                                       .ToList();
            return OkRequest(users);
        }

        [Route("{userId:guid}")]
        [HttpGet]
        public IHttpActionResult Get(Guid userId)
        {
            var user = _getUserService.GetUser(userId);
            return OkRequest(new UserData(user));
        }


        [HttpPost]
        public IHttpActionResult Create(Guid userId, [FromBody] UserDTO model)
        {
            var user = _createUserService.Create(userId, model.Name, model.Email, model.Type, model.AnnualSalary, model.Tags);
            return CreatedAt(new UserData(user), new { user.Id }, "GetById");
        }

        [HttpPut]
        public IHttpActionResult Update(Guid userId, [FromBody] UserDTO model)
        {
            var user = _getUserService.GetUser(userId);
            if (user == null)
            {
                return RecordNotFound();
            }
            _updateUserService.Update(user, model.Name, model.Email, model.Type, model.AnnualSalary, model.Tags);
            return OkRequest(new UserData(user));
        }

        [Route("{userId:guid}/delete")]
        [HttpDelete]
        public IHttpActionResult Delete(Guid userId)
        {
            var user = _getUserService.GetUser(userId);
            if (user == null) return RecordNotFound();

            _deleteUserService.Delete(user);
            return NoContentResponse();
        }

        [Route("clear")]
        [HttpDelete]
        public IHttpActionResult DeleteAll()
        {
            _deleteUserService.DeleteAll();
            return NoContentResponse();
        }

        [Route("list/tag")]
        [HttpGet]
        public IHttpActionResult GetByTag(string tag)
        {
            throw new NotImplementedException();
        }
    }
}