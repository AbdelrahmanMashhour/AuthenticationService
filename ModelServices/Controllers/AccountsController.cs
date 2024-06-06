using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReposatoryPatternWithUOW.Core.DTOs;
using ReposatoryPatternWithUOW.Core.Interfaces;

namespace ModelServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public AccountsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpDto obj)
        {
            var result=await unitOfWork.UserReposatory.SignUpAsync(obj);
            if (result)
            {
               await unitOfWork.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto obj)
        {
            var result = await unitOfWork.UserReposatory.LoginAsync(obj);
            return Ok(result);
        }

        [HttpPost("SendCode")]
        public async Task<IActionResult> SendConfirmationCode(SendCodeDto sendCodeDto)
        {
            var result = await unitOfWork.UserReposatory.SendVerficationCode(sendCodeDto.Email, sendCodeDto.Reset is null or false ? false : true);
            if (result is null)
                return NotFound();
            
            return Ok();

        }
        [HttpPost("ValidateEmailVerificationCode")]
        public async Task<IActionResult> ValidateConfirmationCode(ValidationCodeDto VCD)
        {
            
            var result = await unitOfWork.UserReposatory.ValidateCode(VCD.Email, VCD.Code);
            await unitOfWork.SaveChangesAsync();
            if (!result)
                return Forbid();
            return Ok();

        }

        [HttpDelete("SignOut")]
        public async Task<IActionResult> SignOut(SignOutDto obj)
        {
            var result = await unitOfWork.UserReposatory.SignOut(obj.Email, obj.refreshToken);
            return result ? Ok() : BadRequest();
        }

    }
}
