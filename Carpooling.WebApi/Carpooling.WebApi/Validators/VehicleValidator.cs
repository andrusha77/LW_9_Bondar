using FluentValidation;
using Carpooling.WebApi.Models;

namespace Carpooling.WebApi.Validators;

public class VehicleValidator : AbstractValidator<Vehicle>
{
    public VehicleValidator()
    {
        RuleFor(v => v.OwnerUserId).GreaterThan(0);
        RuleFor(v => v.Make).NotEmpty().MinimumLength(2);
        RuleFor(v => v.Model).NotEmpty();
        RuleFor(v => v.PlateNumber)
            .NotEmpty(); // Regex-вимога
    }
}
