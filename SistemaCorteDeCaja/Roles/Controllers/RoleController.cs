using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaCorteDeCaja.Models;
using SistemaCorteDeCaja.Roles.Controllers.DTOs.Request;
using SistemaCorteDeCaja.Roles.Controllers.DTOs.Response;
using SistemaCorteDeCaja.Roles.Services;
using SistemaCorteDeCaja.Shared.DTOs.Responses;

namespace SistemaCorteDeCaja.Roles.Controllers
{
    [ApiController]
    [Route("api/roles")]
    [Authorize]
    public class RoleController(RoleService roleService, IMapper mapper) : ControllerBase
    {
        private readonly RoleService _roleService = roleService;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<IActionResult> GetRoles([FromQuery] GetRolesQueryParams queryParams)
        {
            List<Role> roles = await _roleService.GetRoles(queryParams);

            var data = _mapper.Map<List<RoleDto>>(roles);

            SuccessResponseDto response = new() { Data = data };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole(int id)
        {
            Role? role = await _roleService.GetRoleById(id);
            IResponse response;

            if (role == null)
            {
                response = new ErrorResponseDto { Title = "Role Not Found" };

                return NotFound(response);
            }

            var data = _mapper.Map<RoleDto>(role);
            response = new SuccessResponseDto { Data = data };

            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest roleToCreate)
        {
            Role role = _mapper.Map<Role>(roleToCreate);

            await _roleService.CreateRole(role);

            return Created();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] UpdateRoleRequest roleToUpdate)
        {
            Role? role = await _roleService.GetRoleById(id);
            IResponse response;


            if (role == null)
            {
                response = new ErrorResponseDto { Title = "Role Not Found" };

                return NotFound(response);
            }

            int affectedRows = await _roleService.UpdateRole(role, roleToUpdate);

            response = new SuccessResponseDto
            {
                Data = new { affectedRows }
            };


            return Ok(response);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            Role? role = await _roleService.GetRoleById(id);
            IResponse response;


            if (role == null)
            {
                response = new ErrorResponseDto { Title = "Role Not Found" };

                return NotFound(response);
            }

            int affectedRows = await _roleService.DeleteRole(role);

            response = new SuccessResponseDto
            {
                Data = new { affectedRows }
            };


            return Ok(response);
        }

    }
}
