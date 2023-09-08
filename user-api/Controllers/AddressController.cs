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
    [Route("api/address")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        APIResponse _response;

        public AddressController(IAddressRepository addressRepository , IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
            _response = new APIResponse();
        }

        [HttpPost,Authorize]
        public async Task<ActionResult<APIResponse>> Post([FromBody] AddressDTO addressDTO)
        {
            var userId = GetUserIdFromClaims();
            if (addressDTO == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Message = "Please enter the Address details";
                return _response;
            }
            Address address = _mapper.Map<Address>(addressDTO);
            address.UserId = userId;
            address.CreatedAt = DateTime.Now;
            address.UpdatedAt = DateTime.Now;
            await _addressRepository.CreateAsync(address);
            _response.Message = " Address Added successfully";
            _response.Data = address;
            return _response;
        }

        [HttpPut("{id:Guid}"), Authorize]
        public async Task<ActionResult<APIResponse>> Put(Guid id, [FromBody] AddressDTO addressDTO)
        {
            Address address = await _addressRepository.GetAsync(id);
            if (address == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Message = "Address not found";
                return _response;
            }
            if (!string.IsNullOrEmpty(addressDTO!.Place))
            {
                address!.Place = addressDTO.Place;
            }

            if (!string.IsNullOrEmpty(addressDTO!.City))
            {
                address!.City = addressDTO.City;
            }

            if (!string.IsNullOrEmpty(addressDTO!.State))
            {
                address!.State = addressDTO.State;
            }

            if (addressDTO.PinCode != 0)
            {
                address!.PinCode = addressDTO.PinCode;
            }
            address!.UpdatedAt = DateTime.Now;
            
            await _addressRepository.UpdateAsync(address);
            _response.Message = "Address Updated successfully";
            _response.Data = address;
            return _response;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<APIResponse>> Get([FromQuery]Guid id)
        {
            var userId = GetUserIdFromClaims();
            if(id != Guid.Empty)
            {
                Address address = await _addressRepository.GetAsync(id);
                if (address == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.Message = "Address not found";
                    return _response;
                }
                _response.Data = address;
            }
            else
            {
                List<Address> addresses = _addressRepository.GetAddressAsync(userId);
                if (addresses == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.Message = "User Don't have any Address";
                    return _response;
                }
                _response.Data = addresses;
            }
            
            _response.Message = "Address Details";
            return _response;
        }

        [HttpDelete("{id:Guid}"), Authorize]
        public async Task<ActionResult<APIResponse>> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Message = "Please enter the valid Id";
                return _response;
            }
            Address address = await _addressRepository.GetAsync(id);
            if (address == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Message = "Address not found";
                return _response;
            }
            await _addressRepository.DeleteAsync(address);
            _response.Message = "Address Deleted";
            _response.Data = address;
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
