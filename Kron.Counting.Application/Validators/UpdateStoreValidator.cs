using FluentValidation;
using Kron.Counting.Application.DTOs.Requests;

namespace Kron.Counting.Application.Validators;

public class UpdateStoreValidator : AbstractValidator<UpdateStoreRequestDto>
{
    public UpdateStoreValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(500);

        RuleFor(x => x.StoreType)
            .MaximumLength(100);

        RuleFor(x => x.Region)
            .MaximumLength(100);
    }
}