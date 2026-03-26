    using global::WebAPI_Project.Services;
    using Microsoft.AspNetCore.Http.HttpResults;
    using Microsoft.AspNetCore.Mvc;
    using ProductsAPI.Dto;
namespace ProductsAPI.Controllers
{


    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {

        private readonly TokenService _tokenService;

        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;

        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequestDto request)
        {

            if (request.UserName == "admin" && request.Password == "password")
            {

                var token = _tokenService.CreateToken(request.UserName);

                return Ok(new
                {
                    token,
                });

            }

            return Unauthorized();
        }
    }
    
}
