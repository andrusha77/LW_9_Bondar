using Microsoft.AspNetCore.Mvc;
using Carpooling.WebApi.Models;
using Carpooling.WebApi.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Carpooling.WebApi.Enum;
namespace Carpooling.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleService _vehicleService;
    public VehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<Vehicle>> GetAll()
    {
        var item = await _vehicleService.GetAllAsync();
        if(item== null) return NotFound();
        return Ok(item);
    }
    [Authorize(Roles = $"{nameof(Roles.Manager)}, {nameof(Roles.Admin)},{nameof(Roles.Super)}")]
    [HttpGet("{id}")]  
    public async Task<ActionResult<Vehicle>> GetById(string id)
    {
        var item = await _vehicleService.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }
    [Authorize(Roles = $"{nameof(Roles.Manager)}, {nameof(Roles.Admin)},{nameof(Roles.Super)}, {nameof(Roles.User)}")]
    [HttpPost]
    public async Task<ActionResult<Vehicle>> Create([FromBody] Vehicle model)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        if (model.OwnerUserId <= 0) return BadRequest("OwnerUserId must be > 0");
        if (string.IsNullOrWhiteSpace(model.Make) || model.Make.Length < 2) return BadRequest("Make min length 2");
        if (string.IsNullOrWhiteSpace(model.Model)) return BadRequest("Model is required");

        await _vehicleService.CreateAsync(model);
        return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
    }
    [Authorize(Roles = $"{nameof(Roles.Admin)}, {nameof(Roles.Manager)},{nameof(Roles.Super)}")]
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, Vehicle input)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        if (input.OwnerUserId <= 0) return BadRequest("OwnerUserId must be > 0");
        if (string.IsNullOrWhiteSpace(input.Make) || input.Make.Length < 2) return BadRequest("Make min length 2");
        if (string.IsNullOrWhiteSpace(input.Model)) return BadRequest("Model is required");

        var item = await _vehicleService.UpdateAsync(id, input);
        if(!item) return NotFound();
        return Ok(item);
    }
    [Authorize(Roles = $"{nameof(Roles.Manager)}, {nameof(Roles.Admin)},{nameof(Roles.Super)}, {nameof(Roles.User)}")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var item = await _vehicleService.DeleteAsync(id);
        if (!item) return NotFound();
        
        return NoContent();
    }
}
