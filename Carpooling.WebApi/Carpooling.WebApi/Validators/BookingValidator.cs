using FluentValidation;
using Carpooling.WebApi.Models;

namespace Carpooling.WebApi.Validators;

public class BookingValidator : AbstractValidator<Booking>
{
    public BookingValidator()
    {
        RuleFor(b => b.RideId).GreaterThan(0);
        RuleFor(b => b.PassengerUserId).GreaterThan(0);
        RuleFor(b => b.Seats).GreaterThan(0);
    }
}
