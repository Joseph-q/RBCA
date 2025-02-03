using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SistemaCorteDeCaja.Models;
using SistemaCorteDeCaja.Permissions.DTOs.Request;
using SistemaCorteDeCaja.Permissions.DTOs.Response;
using SistemaCorteDeCaja.Shared.DTOs.Responses;

namespace SistemaCorteDeCaja.Permissions
{
    [ApiController]
    [Route("api/permissions")]
    public class PermissionController(PermissionService permissionService, IMapper mapper) : ControllerBase
    {
        private readonly PermissionService _permissionService = permissionService;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<IActionResult> GetPermissions([FromQuery] GetPermissionsQueryParams queryParams)
        {
            List<Permission> permissions = await _permissionService.GetPermissions(queryParams);
            List<PermissionDto> data = _mapper.Map<List<PermissionDto>>(permissions);

            SuccessResponseDto response = new SuccessResponseDto()
            {
                Data = data
            };

            return Ok(response);
        }
    }
}
