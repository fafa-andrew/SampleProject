// Cleanup notes:
// 1. All endpoints now return IHttpActionResult for testability purposes
// 2. I ensured that all methods return the appropriate status codes for REST compatibility
// 3. Re-arranged methods in GET, POST, PUT, Delete order for readability
// 4. Changed the HTTP verb for the 'update' endpoint from POST to PUT
// 5. Removed the route attribute for the 'create' endpoint because the userId param is not needed
// 6. Removed the 'user' suffix from method names because its redundant. We are already in the users controller so no need
// 7. Added try catch blocks for catching and handling execptions. We log exceptions using log4net for simplicity.
// 8. Added the route definitions to the verb defintions where possible for conciseness
// 9. Provided suitable name for GET /users/{userId} endpoint
// 10. Improved readability for the GET /users/list endpoint by abstracting the params to a DTO
// 11. Removed redundant GetByTag method and implemented in in the GET /users/list endpoint
// 12. Implemented async/await for all endpoints because they're IO bound

// Future improvements:
// 1. Use AutoMapper to map properties to the DTOs so not some much code is needed when when DB properties change
// 2. Move controller logic into command and query classes using MediatR so the controller is thinned up some more.
// 3. Add authorization to safeguard the endpoint

//Callout information:
//It is my understanding that the endpoints in the task folder of postman need to work as they are without me changing them.
//However, in the case of the POST request, it's not ideal to have the client determine the ID of the record
//being created in the dataase. However, seeing as this is a test and those are the requirements, I will leave it as is.
//
//Furthermore, requests that update a record completely such us the 'update' endpoint should use the PUT verb but again, the postman collection is using
//POST so I'm leaving that as it is as well because of the tests requirements.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Core.Services.Users;
using log4net;
using WebApi.Models.DataTransferObjects.Users;

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

        [Route("list/tag")]
        [HttpGet, Route("list")]
        public async Task<IHttpActionResult> Get([FromUri] UserListRequestDTO query, CancellationToken ct)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequestResponse(ModelState);

                var usersQuery = await _getUserService.GetUsersAsync(ct, query.Type, query.Name, query.Email);
                if (!string.IsNullOrEmpty(query.Tag)) usersQuery = usersQuery.Where(u => u.Tags.Contains(query.Tag));

                var users = usersQuery
                   .Skip(query.Skip).Take(query.Take)
                   .Select(q => new UserResponseDTO(q))
                   .ToList();

                var response = new UserListResponseDTO
                {
                    Page = query.Skip,
                    PageSize = query.Take,
                    Users = users
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to fetch user list", ex);
                return InternalServerError();
            }        
        }

        [HttpGet, Route("{userId:guid}", Name = "GetById")]
        public async Task<IHttpActionResult> Get(Guid userId, CancellationToken ct)
        {
            try
            {
                var user = await _getUserService.GetUserAync(userId, ct);
                if (user == null) return ResourceNotFoundResponse();

                var userResponse = new UserResponseDTO(user);
                return Ok(userResponse);
            }
            catch (Exception ex)
            {
                _logger.Error("Get user failed", ex);
                return InternalServerError();
            }
        }

        [HttpPost, Route("{userId:guid}/create")]
        public async Task<IHttpActionResult> Create(Guid userId, [FromBody] UserRequestDTO userDTO, CancellationToken ct)
        {
            try
            {
                if (userDTO == null || !ModelState.IsValid) return BadRequestResponse(ModelState);

                var existingUser = await _getUserService.GetUserAync(userId, ct);
                if (existingUser != null) return RecordExistsResponse();

                var user = await _createUserService.CreateAsync(
                    userId, 
                    userDTO.Name, 
                    userDTO.Email, 
                    userDTO.Age,
                    userDTO.Type, 
                    userDTO.AnnualSalary, 
                    userDTO.Tags,
                    ct
                    );
               
                var userResponse = new UserResponseDTO(user);
                return CreatedAtRoute("GetById", new { userId = user.Id }, userResponse);
            }
            catch (Exception ex)
            {
                _logger.Error("Create user failed", ex);
                return InternalServerError();
            }
        }

        [HttpPost, Route("{userId:guid}/update")]
        public async Task<IHttpActionResult> Update(Guid userId, [FromBody] UserRequestDTO userDTO, CancellationToken ct)
        {
            try
            {
                if (userDTO == null || !ModelState.IsValid) return BadRequestResponse(ModelState);

                var user = await _getUserService.GetUserAync(userId, ct);
                if (user == null) return ResourceNotFoundResponse();

               await _updateUserService.UpdateAsync(
                    user, 
                    userDTO.Name,
                    userDTO.Email, 
                    userDTO.Age,
                    userDTO.Type, 
                    userDTO.AnnualSalary,
                    userDTO.Tags,
                    ct
                    );

                var userResponse = new UserResponseDTO(user);
                return Ok(userResponse);
            }
            catch (Exception ex)
            {
                _logger.Error("Update user failed", ex);
                return InternalServerError();
            }
        }

        [HttpDelete, Route("{userId:guid}/delete")]
        public async Task<IHttpActionResult> Delete(Guid userId, CancellationToken ct)
        {
            try
            {
                var user = await _getUserService.GetUserAync(userId, ct);
                if (user == null) return ResourceNotFoundResponse();

                await _deleteUserService.DeleteAsync(user, ct);
                return NoContentResponse();
            }
            catch (Exception ex)
            {
                _logger.Error("Delete user failed", ex);
                return InternalServerError();
            }
        }

        //Assumes theres an elevated role called Admin since this deletes all users.
        //Creating roles is out of scope for this test
        [Authorize(Roles = "Admin")]
        [HttpDelete, Route("clear")]
        public async Task<IHttpActionResult> DeleteAll(CancellationToken ct)
        {
            try
            {
                await _deleteUserService.DeleteAllAsync(ct);
                return NoContentResponse();
            }
            catch (Exception ex)
            {
                _logger.Error("Clear user failed", ex);
                return InternalServerError();
            }
        }
    }
}