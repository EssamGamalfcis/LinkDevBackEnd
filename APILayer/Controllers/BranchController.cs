using DomainLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.ICustomServices;

namespace APILayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _customService;
        public BranchController(IBranchService customService)
        {
            _customService = customService;
        }

        [HttpGet("GetAllBranches")]
        public IActionResult GetAllBranches([FromQuery]ComonParam param)
        {
            var obj = _customService.GetAll(param);
            if(obj.success == true)
            {
                return Ok(obj);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("CreateBranch")]
        [Authorize(AuthenticationSchemes = "Bearer" , Roles = "Admin")]
        public IActionResult CreateBranch([FromBody] BranchVM branch)
        {
            if (branch != null)
            {
              var response = _customService.Insert(branch);
                if (response.success)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            else
            {
                return BadRequest("Somethingwent wrong");
            }
        }

        [HttpPost("UpdateBranch")]
        [Authorize(AuthenticationSchemes = "Bearer" , Roles = "Admin")]
        public IActionResult UpdateBranch([FromBody]BranchVM branch)
        {
            if(branch != null)
            {
                var response = _customService.Update(branch);
                if (response.success)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            else
            {
                return BadRequest("Somethingwent wrong");
            }
            
        }

        [HttpPost("DeleteBranch")]
        [Authorize(AuthenticationSchemes = "Bearer" , Roles = "Admin")]
        public IActionResult DeleteBranch([FromBody]long id)
        {
            if (id != 0)
            {
                var response = _customService.Delete(id);
                if (response.success)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            else
            {
                return BadRequest("Something went wrong");
            }
        }

    }
}
