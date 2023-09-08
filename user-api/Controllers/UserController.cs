using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using user_api.Models;
using user_api.Models.DTOs;
using user_api.Repository.IRepository;

namespace user_api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ITokenRepository _tokenRepository;
        private readonly IPasswordEncoderRepository _encoderRepository;
        private readonly IAddressRepository _addressRepository;
        APIResponse _response;

        public UserController(IUserRepository userRepository , IMapper mapper , ITokenRepository tokenRepository , IPasswordEncoderRepository encoderRepository , IAddressRepository addressRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenRepository = tokenRepository;
            _encoderRepository = encoderRepository;
            _addressRepository = addressRepository;
            _response = new();
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> Post([FromBody] UserDTO userDTO)
        {
            if(userDTO == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Message = " Please enter the user details";
                return _response;
            }

            User isUserExists = await _userRepository.VerifyUserAsync(userDTO.Email!);
            if(isUserExists != null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Message = "Email Already Exists";
                return _response;
            }
            
            User user = _mapper.Map<User>(userDTO);
            user.CreatedAt = DateTime.Now;
            user.UpdatedAt = DateTime.Now;

            EncodedPassword encodedPassword = _encoderRepository.GetPassword(userDTO!.Password!);
            user.PasswordHash = encodedPassword.PasswordHash;
            user.PasswordSalt = encodedPassword.PasswordSalt;

            await _userRepository.CreateAsync(user);
            var token_data = new
            {
                token = _tokenRepository.GetToken(user)
            };

            _response.Message = "User Registered Successfully";
            _response.Data = token_data;
            return _response;
        }

        [HttpPut, Authorize]
        public async Task<ActionResult<APIResponse>> Put([FromBody] UserUpdateDTO userDTO)
        {
            var userId = GetUserIdFromClaims();
            User user = await _userRepository.GetAsync(userId);
            if(user == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Message = "User not found";
            }

            if (!string.IsNullOrEmpty(userDTO.Name))
            {
                user!.Name = userDTO.Name;
            }
            if (!string.IsNullOrEmpty(userDTO.Email))
            {
                user!.Email = userDTO.Email;
            }
            if (!string.IsNullOrEmpty(userDTO.PhoneNumber))
            {
                user!.PhoneNumber = userDTO.PhoneNumber;
            }
            if (!string.IsNullOrEmpty(userDTO.Designation))
            {
                user!.Designation = userDTO.Designation;
            }
            user!.UpdatedAt = DateTime.Now;
           
            await _userRepository.UpdateAsync(user);
            _response.Message = "User Updated Succesfully";
            return _response;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<APIResponse>> Get()
        {
            var userId = GetUserIdFromClaims();
            User user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Message = "User not found";
                return _response;
            }

            List<Address> addresses = _addressRepository.GetAddressAsync(userId);
            var userDetails = new
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Designation = user.Designation,
                Address = addresses,
            };

            _response.Message = "User Details";
            _response.Data = userDetails;
            return _response;
        }

        private Guid GetUserIdFromClaims()
        {
            var user = HttpContext.User;
            string id = user.FindFirst(ClaimTypes.Sid)?.Value!;
            Guid guid = new Guid(id);
            return guid;
        }
    }
}
