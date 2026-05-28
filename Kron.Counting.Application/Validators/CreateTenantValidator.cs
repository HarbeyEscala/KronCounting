using FluentValidation;
using Kron.Counting.Application.DTOs.Requests;

namespace Kron.Counting.Application.Validators;

public class CreateTenantValidator : AbstractValidator<CreateTenantRequestDto>
{
    public CreateTenantValidator()
    {
        RuleFor(x => x.BrandId)
            .NotEmpty()
            .WithMessage("BrandId is required");

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Code is required")
            .MaximumLength(50)
            .WithMessage("Code cannot exceed 50 characters");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(200)
            .WithMessage("Name cannot exceed 200 characters");

        RuleFor(x => x.TimeZone)
            .NotEmpty()
            .WithMessage("TimeZone is required")
            .MaximumLength(100)
            .WithMessage("TimeZone cannot exceed 100 characters");

        RuleFor(x => x.Currency)
            .NotEmpty()
            .WithMessage("Currency is required")
            .MaximumLength(10)
            .WithMessage("Currency cannot exceed 10 characters");

        RuleFor(x => x.Locale)
            .NotEmpty()
            .WithMessage("Locale is required")
            .MaximumLength(20)
            .WithMessage("Locale cannot exceed 20 characters");
    }
}