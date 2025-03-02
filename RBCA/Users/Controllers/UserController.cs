﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RBCA.Users.Controllers.DTOs.Responses;
using SistemaCorteDeCaja.Auth.Atributtes;
using SistemaCorteDeCaja.Auth.Constants;
using SistemaCorteDeCaja.Models;
using SistemaCorteDeCaja.Shared.DTOs.Responses;
using SistemaCorteDeCaja.Users.Controllers.DTOs.Request;
using SistemaCorteDeCaja.Users.Controllers.DTOs.Responses;
using SistemaCorteDeCaja.Users.Services;

namespace SistemaCorteDeCaja.Users.Controllers
{

    [Authorize]
    [PermissionAuthorize]
    [ApiController]
    [Route("api/user")]
    [Produces("application/json")]
    [ProducesResponseType(401)]
    [ProducesResponseType(404, Type = typeof(ErrorResponseDto))]
    public class UserController(UserService uservice, IMapper mapper) : ControllerBase
    {
        private readonly UserService _userservice = uservice;
        private readonly IMapper _mapper = mapper;


        [HttpGet]
        [PermissionPolicy(DefaultActions.Read, DefaultSubjects.Users)]
        [ProducesResponseType(200, Type = typeof(SuccessResponseDto<List<UserWithoutRolesDTO>>))]
        public async Task<IActionResult> GetUsers([FromQuery] GetUsersQueryParams queryParams)
        {
            List<User> users = await _userservice.GetUsers(queryParams);

            var data = _mapper.Map<List<UserDTO>>(users);
            SuccessResponseDto response = new() { Data = data };

            return Ok(response);
        }
        [ProducesResponseType(200, Type = typeof(SuccessResponseDto<UserDTO>))]
        [HttpGet("{id}")]
        [PermissionPolicy(DefaultActions.Read, DefaultSubjects.Users)]
        public async Task<IActionResult> GetUser(int id, [FromQuery] GetUserQueryParams queryParams)
        {
            User? user = await _userservice.GetUserById(id, queryParams);
            IResponse response;

            if (user == null)
            {
                response = new ErrorResponseDto { Title = "Usuario no econtrado" };
                return NotFound(response);
            }

            UserDTO data = _mapper.Map<UserDTO>(user);

            response = new SuccessResponseDto { Data = data };

            return Ok(response);
        }

        [ProducesResponseType(201)]
        [HttpPost]
        [PermissionPolicy(DefaultActions.Create, DefaultSubjects.Users)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest user)
        {
            await _userservice.CreateUser(user);
            return Created();
        }

        [ProducesResponseType(204)]
        [HttpPut("{id}")]
        [PermissionPolicy(DefaultActions.Update, DefaultSubjects.Users)]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest userToUpdate)
        {
            User? userDb = await _userservice.GetUserById(id);

            if (userDb == null)
            {
                return NotFound(new ErrorResponseDto { Title = "Usuario no econtrado" });
            }

            await _userservice.UpdateUser(userDb, userToUpdate);
            return NoContent();
        }

        [ProducesResponseType(204)]
        [HttpDelete("{id}")]
        [PermissionPolicy(DefaultActions.Delete, DefaultSubjects.Users)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            User? user = await _userservice.GetUserById(id);


            if (user == null)
            {
                return NotFound(new ErrorResponseDto { Title = "Usuario no econtrado" });
            }

            await _userservice.DeleteUser(user);

            return NoContent();
        }
    }

}
