using Maqha.Utilities.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maqha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult CreateResponse<T>(ResponseDTO<T>response) where T : class
        {
            //check if response is successful
            if (response.IsSuccess)
                return Ok(response);
            //check if response is not successful //Switch on ErrorCode
            return response.ErrorCode switch
            {
                ErrorCodes.ValidationError => BadRequest(response),
                ErrorCodes.NotFound => NotFound(response),
                ErrorCodes.Unauthorized => Unauthorized(response),
                ErrorCodes.DuplicateError => Conflict(response),
                ErrorCodes.Forbidden => StatusCode(StatusCodes.Status403Forbidden, response),
                ErrorCodes.BadRequest => BadRequest(response),
                _ => BadRequest(response)
            };
        }
        //Return UserId from the current user
        protected string GetUserId()
        {
            return User?.Claims.FirstOrDefault(x => x.Type == "_e")?.Value ?? string.Empty;
        }
    }
}
