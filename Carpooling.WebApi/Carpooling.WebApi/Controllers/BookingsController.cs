using Carpooling.WebApi.Enum;
using Carpooling.WebApi.Interfaces;
using Carpooling.WebApi.Models;
using Carpooling.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carpooling.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;
    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }
    [Authorize]
    [HttpGet]
    public async  Task<ActionResult<Booking>> GetAll()
    {
        var item = await _bookingService.GetAllAsync();
        if(item is null)  return NotFound();
        
        return Ok(item);
    }
    [Authorize(Roles = $"{nameof(Roles.Manager)}, {nameof(Roles.Admin)},{nameof(Roles.Super)}")]

    [HttpGet("{id}")]
    public async Task<ActionResult<Booking>> GetById(string id)
    {
        var item = _bookingService.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }
    [Authorize(Roles = $"{nameof(Roles.Manager)}, {nameof(Roles.Admin)},{nameof(Roles.Super)}, {nameof(Roles.User)}")]
    [HttpPost]
    public async Task<ActionResult<Booking>> Create(Booking model)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        if (model.RideId <= 0) return BadRequest("RideId must be > 0");
        if (model.PassengerUserId <= 0) return BadRequest("PassengerUserId must be > 0");
        if (model.Seats <= 0) return BadRequest("Seats must be > 0");
        await _bookingService.CreateAsync(model);
        return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
    }
    [Authorize(Roles = $"{nameof(Roles.Admin)}, {nameof(Roles.Manager)},{nameof(Roles.Super)}")]
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, Booking input)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        if (input.RideId <= 0) return BadRequest("RideId must be > 0");
        if (input.PassengerUserId <= 0) return BadRequest("PassengerUserId must be > 0");
        if (input.Seats <= 0) return BadRequest("Seats must be > 0");

        var item = await _bookingService.GetByIdAsync(id);
        return Ok(item);
    }
    [Authorize(Roles = $"{nameof(Roles.Manager)}, {nameof(Roles.Admin)},{nameof(Roles.Super)}, {nameof(Roles.User)}")]
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var item = _bookingService.GetByIdAsync(id);
        if (item is null) return NotFound();
        return NoContent();
    }
}
