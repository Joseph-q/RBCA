﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RBCA.Shared.DTOs.Responses;
using SistemaCorteDeCaja.Auth.Atributtes;
using SistemaCorteDeCaja.Auth.Constants;
using SistemaCorteDeCaja.Models;
using SistemaCorteDeCaja.Roles.Controllers.DTOs.Request;
using SistemaCorteDeCaja.Roles.Controllers.DTOs.Response;
using SistemaCorteDeCaja.Roles.Services;
using SistemaCorteDeCaja.Shared.DTOs.Responses;

namespace SistemaCorteDeCaja.Roles.Controllers
{
    [ApiController]
    [Route("api/roles")]
    [Produces("application/json")]
    [ProducesResponseType(401)]
    [ProducesResponseType(404, Type = typeof(ErrorResponseDto))]
    [Authorize]
    [PermissionAuthorize]
    public class RoleController(RoleService roleService, IMapper mapper) : ControllerBase
    {
        private readonly RoleService _roleService = roleService;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(SuccessResponseDto<List<RoleDto>>))]
        [PermissionPolicy(DefaultActions.Read, DefaultSubjects.Roles)]
        public async Task<IActionResult> GetRoles([FromQuery] GetRolesQueryParams queryParams)
        {
            List<Role> roles = await _roleService.GetRoles(queryParams);

            var data = _mapper.Map<List<RoleDto>>(roles);

            SuccessResponseDto response = new() { Data = data };

            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(SuccessResponseDto<RoleDto>))]
        [PermissionPolicy(DefaultActions.Read, DefaultSubjects.Roles)]
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


        [ProducesResponseType(201)]
        [HttpPost]
        [PermissionPolicy(DefaultActions.Create, DefaultSubjects.Roles)]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest roleToCreate)
        {
            Role role = _mapper.Map<Role>(roleToCreate);

            await _roleService.CreateRole(role);

            return Created();
        }

        [ProducesResponseType(201, Type = typeof(SuccessResponseDto<RowsAffectedDTO>))]
        [HttpPut("{id}")]
        [PermissionPolicy(DefaultActions.Update, DefaultSubjects.Roles)]
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

        [ProducesResponseType(201, Type = typeof(SuccessResponseDto<RowsAffectedDTO>))]
        [HttpDelete("{id}")]
        [PermissionPolicy(DefaultActions.Delete, DefaultSubjects.Roles)]
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

        /// <response code="201">Add permitions to role, if you add many roles affeted rows will be 1</response>
        [HttpPost("{id}/permissions")]
        [ProducesResponseType(201, Type = typeof(SuccessResponseDto<RowsAffectedDTO>))]
        [PermissionPolicy(DefaultActions.Update, DefaultSubjects.Roles)]
        public async Task<IActionResult> AddPermissionsToRole(int id, [FromBody] AddPermissionToRoleRequest permissionRequest)
        {
            Role? role = await _roleService.GetRoleById(id);
            IResponse response;
            if (role == null)
            {
                response = new ErrorResponseDto { Title = "Role Not Found" };
                return NotFound(response);
            }

            int affectedRows = await _roleService.AddPermissionsToRole(role, permissionRequest.permissionsId);

            response = new SuccessResponseDto
            {
                Data = new { affectedRows }
            };
            return Ok(response);
        }

    }
}
