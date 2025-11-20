using Carpooling.WebApi.Enum;
using Carpooling.WebApi.Interfaces;
using Carpooling.WebApi.Models;
using Carpooling.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace Carpooling.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    public async Task<ActionResult<User>> GetAll()
    {
        var item = await _userService.GetAllAsync();
        return Ok(item);
    }
    [Authorize(Roles=$"{nameof(Roles.Manager)}, {nameof(Roles.Admin)},{nameof(Roles.Super)}")]
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetById(string id)
    {
        var item = await _userService.GetByIdAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [Authorize(Roles = $"{nameof(Roles.Admin)}, {nameof(Roles.Manager)},{nameof(Roles.Super)}")]
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, User input)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var item = await _userService.UpdateAsync(id, input);
        if(!item) return NotFound();
        return Ok(item);
    }
    [Authorize(Roles = $"{nameof(Roles.Admin)}, {nameof(Roles.Manager)},{nameof(Roles.Super)}")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var item = await _userService.DeleteAsync(id);
        if (!item) return NotFound();
        return NoContent();
    }

    [Authorize(Roles = $"{nameof(Roles.Super)}")]
    [HttpPost("setRoles")]
    public async Task<IActionResult> SetRoles(string id, Roles roles)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound("User not found");

        user.Role = roles;
        await _userService.UpdateAsync(user.Id, user);

        return Ok(new { username = user.Name, roles = user.Role, rolesValue = (int)user.Role });
    }

    [Authorize(Roles = $"{nameof(Roles.Super)}")]
    [HttpPost("addRole")]
    public async Task<IActionResult> AddRole(string id, Roles role)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();

        user.Role |= role; // додаємо прапорець
        await _userService.UpdateAsync(user.Id, user);

        return Ok(new { username = user.Name, roles = user.Role, rolesValue = (int)user.Role });
    }

    [Authorize(Roles = $"{nameof(Roles.Super)}")]
    [HttpPost("removeRole")]
    public async Task<IActionResult> RemoveRole(string id, Roles role)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();

        user.Role &= ~role; // знімаємо прапорець
        await _userService.UpdateAsync(user.Id,user);

        return Ok(new { username = user.Name, roles = user.Role, rolesValue = (int)user.Role });
    }
}
