using FluentValidation;

namespace BulkDataImport.Api;

/// <summary>
/// Validator for ValidationRequest using FluentValidation
/// </summary>
public class ValidationRequestValidator : AbstractValidator<ValidationRequest>
{
    public ValidationRequestValidator()
    {
        // Rule for Name property
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MinimumLength(2)
            .WithMessage("Name must be at least 2 characters long")
            .MaximumLength(50)
            .WithMessage("Name cannot exceed 50 characters")
            .Matches(@"^[a-zA-Z\s]+$")
            .WithMessage("Name can only contain letters and spaces");

        // Rule for Email property
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Email must be a valid email address")
            .MaximumLength(100)
            .WithMessage("Email cannot exceed 100 characters");

        // Rule for Age property
        RuleFor(x => x.Age)
            .GreaterThanOrEqualTo(18)
            .WithMessage("Age must be at least 18")
            .LessThanOrEqualTo(120)
            .WithMessage("Age cannot exceed 120");
    }
}

