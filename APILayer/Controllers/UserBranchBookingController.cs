using DomainLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.ICustomServices;

namespace APILayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UserBranchBookingController : ControllerBase
    {
        private readonly IUserBranchBookingService _customService;
        public UserBranchBookingController(IUserBranchBookingService customService)
        {
            _customService = customService;
        }
        [HttpGet("GetAllBranches")]
        public async Task<IActionResult> GetAllBranches([FromQuery] ComonParam param)
        {
            var obj = await _customService.GetAll(param);
            if (obj.success == true)
            {
                return Ok(obj);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost("BookBranch")]
        public async Task<IActionResult> BookBranch([FromBody] UserBranchBookingVM param)
        {
            var response = await _customService.Insert(param);
            if (response.success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        } 

    }
}
