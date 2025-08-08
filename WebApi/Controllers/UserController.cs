// Cleanup notes:
// 1. All endpoints now return IHttpActionResult for testability purposes
// 2. I ensured that all methods return the appropriate status codes for REST compatibility
// 3. Re-arranged methods in GET, POST, PUT, Delete order for readability
// 4. Changed the HTTP verb for /update from POST to PUT
// 5. Removed the route attribute for /create endpoints because the userId param is not needed
// 6. Removed the 'user' suffix from method names because its redundant. We are already in the users controller so no need
// 7. Added try catch blocks for catching and handling execptions. We log exceptions using log4net for simplicity.
// 8. Added the route definitions to the verb defintions for conciseness

using System;
using System.Linq;
using System.Web.Http;
using BusinessEntities;
using Core.Services.Users;
using log4net;
using WebApi.Models.Users;

namespace WebApi.Controllers
{
    [RoutePrefix("users")]
    public class UserController : BaseApiController
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(UserController));

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

        [HttpGet, Route("list")]
        public IHttpActionResult Get(int skip, int take, UserTypes? type = null, string name = null, string email = null)
        {
            try
            {
                var users = _getUserService.GetUsers(type, name, email)
                                   .Skip(skip).Take(take)
                                   .Select(q => new UserData(q))
                                   .ToList();
                
                return OkResponse(users);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return InternalServerErrorResponse();
            }        
        }

        [HttpGet, Route("{userId:guid}")]
        public IHttpActionResult Get(Guid userId)
        {
            try
            {
                var user = _getUserService.GetUser(userId);
                return OkResponse(new UserData(user));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return InternalServerErrorResponse();
            }
        }

        [HttpPost]
        public IHttpActionResult Create(Guid userId, [FromBody] UserDTO model)
        {
            try
            {
                var user = _createUserService.Create(userId, model.Name, model.Email, model.Type, model.AnnualSalary, model.Tags);
                return Created(new UserData(user), new { user.Id }, "GetById");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return InternalServerErrorResponse();
            }
        }

        [HttpPut, Route("{userId:guid}")]
        public IHttpActionResult Update(Guid userId, [FromBody] UserDTO model)
        {
            try
            {
                var user = _getUserService.GetUser(userId);
                if (user == null)
                {
                    return NotFoundResponse();
                }

                _updateUserService.Update(user, model.Name, model.Email, model.Type, model.AnnualSalary, model.Tags);
                return OkResponse(new UserData(user));
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return InternalServerErrorResponse();
            }
        }

        [HttpDelete, Route("{userId:guid}/delete")]
        public IHttpActionResult Delete(Guid userId)
        {
            try
            {
                var user = _getUserService.GetUser(userId);
                if (user == null) return NotFoundResponse();

                _deleteUserService.Delete(user);
                return NoContentResponse();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return InternalServerErrorResponse();
            }
        }

        [HttpDelete, Route("clear")]
        public IHttpActionResult DeleteAll()
        {
            try
            {
                _deleteUserService.DeleteAll();
                return NoContentResponse();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return InternalServerErrorResponse();
            }
        }

        [HttpGet, Route("list/tag")]
        public IHttpActionResult GetByTag(string tag)
        {
            try
            {
                //todo: wil be implemented soon
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return InternalServerErrorResponse();
            }
        }
    }
}