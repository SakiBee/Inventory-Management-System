using IMS.DTOs.Auth;
using IMS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Controllers
{
    /// <summary>
    /// Manages Auth-related operations
    /// </summary>
    [ApiController]
    [Route("/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="dto">User register payload</param>
        /// <returns>The user register</returns>
        /// <response code="201">User registered successfully</response>
        /// <response code="400">Invalid input</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterRequestDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            //return Ok(result);
            return CreatedAtAction(nameof(Register), result);
        }


        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="dto">User login payload</param>
        /// <returns>The user login</returns>
        /// <response code="201">User logged in successfully</response>
        /// <response code="400">Invalid input</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(result);
        }


        /// <summary>
        /// Logout user
        /// </summary>
        /// <param name="dto">User logout payload</param>
        /// <returns>The user logout</returns>
        /// <response code="201">User logged out successfully</response>
        /// <response code="400">Invalid input</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new { message = "Logged out successfully." });
        }
    }
}
