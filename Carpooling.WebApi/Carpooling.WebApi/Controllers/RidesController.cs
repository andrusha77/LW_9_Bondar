using Carpooling.WebApi.Enum;
using Carpooling.WebApi.Interfaces;
using Carpooling.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carpooling.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RidesController : ControllerBase
{
    private readonly IRideService _rideService;
    public RidesController(IRideService rideService)
    {
        _rideService = rideService;
    }
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<Ride>> GetAll()
    {

        var item = await _rideService.GetAllAsync();
        if (item == null) return NotFound();
        return Ok(item);
    }
    [Authorize(Roles = $"{nameof(Roles.Manager)}, {nameof(Roles.Admin)},{nameof(Roles.Super)}")]
    [HttpGet("{id}")]
    public async Task<ActionResult<Ride>> GetById(string id)
    {
        var item = await _rideService.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }



    [Authorize(Roles = $"{nameof(Roles.Manager)}, {nameof(Roles.Admin)},{nameof(Roles.Super)}, {nameof(Roles.User)}")]
    [HttpPost]
    public async Task<ActionResult<Ride>> Create([FromBody] Ride model)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        if (string.IsNullOrWhiteSpace(model.From) || string.IsNullOrWhiteSpace(model.To)) return BadRequest("From/To required");
        if (model.SeatsTotal <= 0) return BadRequest("SeatsTotal must be > 0");
        if (model.DepartureTime == default) model.DepartureTime = DateTime.UtcNow.AddHours(1);

        await _rideService.CreateAsync(model);
        return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
    }





    [Authorize(Roles = $"{nameof(Roles.Admin)}, {nameof(Roles.Manager)},{nameof(Roles.Super)}")]
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, Ride input)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        if (string.IsNullOrWhiteSpace(input.From) || string.IsNullOrWhiteSpace(input.To)) return BadRequest("From/To required");
        if (input.SeatsTotal <= 0) return BadRequest("SeatsTotal must be > 0");

        var item = await _rideService.UpdateAsync(id, input);
        if(!item) return NotFound();
        return Ok(item);
    }
    [Authorize(Roles = $"{nameof(Roles.Manager)}, {nameof(Roles.Admin)},{nameof(Roles.Super)}, {nameof(Roles.User)}")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var item = await _rideService.DeleteAsync(id);
        if (!item) return NotFound();
        return NoContent();
    }
}
