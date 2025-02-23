using CargoPay.Core.DTOs;
using CargoPay.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CargoPay.API.Controllers
{
    /// <summary>
    /// Controller responsible for handling authentication-related requests.
    /// </summary>
    [ApiController]
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authService">Authentication service dependency.</param>
        /// <param name="logger">Logger instance for logging authentication actions.</param>
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userDto">User registration data transfer object.</param>
        /// <returns>Returns authentication response upon successful registration.</returns>
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] UserRegisterDto userDto)
        {
            try
            {
                var result = await _authService.RegisterAsync(userDto);
                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Authenticates a user and provides a token if successful.
        /// </summary>
        /// <param name="loginDto">User login data transfer object.</param>
        /// <returns>Returns authentication response upon successful login.</returns>
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] UserLoginDto loginDto)
        {
            try
            {
                var result = await _authService.LoginAsync(loginDto);
                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Initiates a password reset request for a user.
        /// </summary>
        /// <param name="request">Forgot password request data transfer object.</param>
        /// <returns>Returns a reset token if the email exists in the system.</returns>
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto request)
        {
            try
            {
                var resetToken = await _authService.GeneratePasswordResetTokenAsync(request.Email);
                return Ok(new { message = "If your email exists in our system, use the following token to access the reset password.", token = resetToken });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Resets the user's password using a provided reset token.
        /// </summary>
        /// <param name="request">Reset password request data transfer object.</param>
        /// <returns>Returns success message if password is reset successfully.</returns>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
        {
            try
            {
                await _authService.ResetPasswordAsync(request.Token, request.NewPassword);
                return Ok(new { message = "Password has been reset successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
