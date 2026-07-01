using FluentValidation;
using PRN232.LMS.Services.RequestModels;

namespace PRN232.LMS.Services.Validation
{
    /// <summary>
    /// FluentValidation validator for <see cref="CreateStudentRequest"/>.
    /// Demonstrates advanced validation rules beyond DataAnnotations.
    /// </summary>
    public class CreateStudentRequestValidator : AbstractValidator<CreateStudentRequest>
    {
        public CreateStudentRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("FullName is required.")
                .MinimumLength(2).WithMessage("FullName must be at least 2 characters.")
                .MaximumLength(100).WithMessage("FullName must not exceed 100 characters.")
                .Matches(@"^[\p{L}\s]+$").WithMessage("FullName must contain only letters and spaces.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .MaximumLength(100).WithMessage("Email must not exceed 100 characters.")
                .EmailAddress().WithMessage("Invalid email format.")
                .Must(email => email.EndsWith("@fpt.edu.vn") || email.EndsWith("@fe.edu.vn"))
                .WithMessage("Email must be an FPT University email (@fpt.edu.vn or @fe.edu.vn).");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("DateOfBirth is required.")
                .Must(BeAtLeast15YearsOld).WithMessage("Student must be at least 15 years old.")
                .Must(BeReasonableAge).WithMessage("DateOfBirth seems unrealistic (maximum age is 100 years).");

            RuleFor(x => x.StudentCode)
                .Matches(@"^[A-Z]{2}\d{5}$")
                .WithMessage("Student code must follow FPTU format: 2 uppercase letters + 5 digits (e.g. SE19886).")
                .When(x => !string.IsNullOrEmpty(x.StudentCode));

            RuleFor(x => x.Phone)
                .Matches(@"^\+?[\d\s\-\(\)]{7,15}$")
                .WithMessage("Invalid phone number format.")
                .When(x => !string.IsNullOrEmpty(x.Phone));
        }

        private bool BeAtLeast15YearsOld(DateTime dateOfBirth)
        {
            return dateOfBirth <= DateTime.Today.AddYears(-15);
        }

        private bool BeReasonableAge(DateTime dateOfBirth)
        {
            return dateOfBirth >= DateTime.Today.AddYears(-100);
        }
    }
}
