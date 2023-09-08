using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using user_api.Models;
using user_api.Repository.IRepository;

namespace user_api.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IPasswordEncoderRepository _passwordEncoderRepository;
        APIResponse _response;
        public AuthenticationController(IUserRepository userRepository, ITokenRepository tokenRepository, IPasswordEncoderRepository passwordEncoderRepository)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _passwordEncoderRepository = passwordEncoderRepository;
            _response = new APIResponse();
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> Post([FromBody] Login login)
        {
            if (login == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "Please enter the email and password";
                return _response;
            }
            User user = await _userRepository.VerifyUserAsync(login.Email!);
            if (user == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "wrong email";
                return _response;
            }
            bool isCorrectPassword = _passwordEncoderRepository.VerfiyPassword(login.Password!, user.PasswordHash!, user.PasswordSalt!);
            if (!isCorrectPassword)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "wrong password";
                return _response;
            }

            var token_data = new
            {
                Token = _tokenRepository.GetToken(user)
            };

            _response.Message = "Login successfull";
            _response.Data = token_data;
            return _response;
        }
    }
}
