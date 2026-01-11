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
        /// Register a new user
        /// </summary>
        /// <param name="dto">User registration payload</param>
        /// <returns>Registration result</returns>
        /// <response code="201">User registered successfully</response>
        /// <response code="400">Invalid input</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="dto">User login payload</param>
        /// <returns>Authentication result containing JWT token</returns>
        /// <response code="200">User logged in successfully</response>
        /// <response code="400">Invalid input</response>
        /// <response code="401">Invalid credentials</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(result);
        }

        /// <summary>
        /// Logout the currently authenticated user
        /// </summary>
        /// <returns>Confirmation message</returns>
        /// <response code="200">User logged out successfully</response>
        /// <response code="401">Unauthorized - user is not logged in</response>
        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Logout()
        {
            // Here you could also revoke tokens if using JWT refresh tokens
            return Ok(new { message = "Logged out successfully." });
        }


    }
}
