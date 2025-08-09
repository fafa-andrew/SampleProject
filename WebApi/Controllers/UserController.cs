// Cleanup notes:
// 1. All endpoints now return IHttpActionResult for testability purposes
// 2. I ensured that all methods return the appropriate status codes for REST compatibility
// 3. Re-arranged methods in GET, POST, PUT, Delete order for readability
// 4. Changed the HTTP verb for the 'update' endpoint from POST to PUT
// 5. Removed the route attribute for the 'create' endpoint because the userId param is not needed
// 6. Removed the 'user' suffix from method names because its redundant. We are already in the users controller so no need
// 7. Added try catch blocks for catching and handling execptions. We log exceptions using log4net for simplicity.
// 8. Added the route definitions to the verb defintions for conciseness
// 9. Provided suitable name for GET /users/{userId} endpoint
// 10. Improved readability for the GET /users/list endpoint by abstracting the params to a DTO
// 11. Removed redundant GetByTag method and implemented in in the GET /users/list endpoint

// Future optimzations:
// 1. Use AutoMapper to map properties to the DTOs so not some much code is needed when when DB properties increase

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

        [HttpGet, Route("list")]
        public async Task<IHttpActionResult> Get([FromUri] UserListRequestDTO query, CancellationToken ct)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

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
                if (user == null) return NotFound();

                var userResponse = new UserResponseDTO(user);
                return Ok(userResponse);
            }
            catch (Exception ex)
            {
                _logger.Error("Get user failed", ex);
                return InternalServerError();
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> Create([FromBody] UserRequestDTO userDTO, CancellationToken ct)
        {
            try
            {
                if (userDTO == null || !ModelState.IsValid) return BadRequest(ModelState);

                //In an ideal situation, We need to check if this new ID doesn't already belong to a user.
                //It will be better if the DB generates unique keys on it's own to prevent this check but that's beyond the scope of this test
                var userId = Guid.NewGuid();
                var user = await _createUserService.CreateAsync(
                    userId, 
                    userDTO.Name, 
                    userDTO.Email, 
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

        [HttpPut, Route("{userId:guid}/update")]
        public async Task<IHttpActionResult> Update(Guid userId, [FromBody] UserRequestDTO userDTO, CancellationToken ct)
        {
            try
            {
                if (userDTO == null || !ModelState.IsValid) return BadRequest(ModelState);

                var user = await _getUserService.GetUserAync(userId, ct);
                if (user == null) return NotFound();

               await _updateUserService.UpdateAsync(
                    user, 
                    userDTO.Name,
                    userDTO.Email, 
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
                if (user == null) return NotFound();

                await _deleteUserService.DeleteAsync(user, ct);
                return NoContent();
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
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Error("Clear user failed", ex);
                return InternalServerError();
            }
        }
    }
}