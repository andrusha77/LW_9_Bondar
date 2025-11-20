using FluentValidation;
using Carpooling.WebApi.Models;

namespace Carpooling.WebApi.Validators;

public class RideValidator : AbstractValidator<Ride>
{
    public RideValidator()
    {
        RuleFor(r => r.DriverUserId).GreaterThan(0);
        RuleFor(r => r.From).NotEmpty();
        RuleFor(r => r.To).NotEmpty();
        RuleFor(r => r.SeatsTotal).GreaterThan(0);
        RuleFor(r => r.Price).GreaterThanOrEqualTo(0);
    }
}
