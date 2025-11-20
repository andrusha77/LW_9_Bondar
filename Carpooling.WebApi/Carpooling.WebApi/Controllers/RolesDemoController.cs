using Carpooling.WebApi.Enum;
using Carpooling.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Carpooling.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesDemoController : ControllerBase
    {
        // Лише користувачі з роллю Admin
        [Authorize(Roles = nameof(Roles.Super))]
        [HttpGet("Super")]
        public IActionResult SuperAccess() => Ok("Super access granted");
        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpGet("admin")]
        public IActionResult OnlyAdmins() => Ok("Admin access granted");

        // Admin або Manager
        [Authorize(Roles = $"{nameof(Roles.Admin)},{nameof(Roles.Manager)}")]
        [HttpGet("management")]
        public IActionResult ManagersAndAdmins() => Ok("Manager/Admin access granted");
        
        [Authorize]
        [HttpGet("whoami")]
        public IActionResult WhoAmI()
        {
            var name = User.Identity?.Name;
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);
            return Ok(new { name, roles });
        }
    }
}
