using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PRN232.LMS.Services.Validation
{
    /// <summary>
    /// Custom validation attribute that validates FPTU-style student codes.
    /// Valid format: 2 uppercase letters followed by 5 digits (e.g., SE19886, CE18793, AI20001).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class FptuStudentCodeAttribute : ValidationAttribute
    {
        private static readonly Regex _regex = new(@"^[A-Z]{2}\d{5}$", RegexOptions.Compiled);

        public FptuStudentCodeAttribute()
        {
            ErrorMessage = "Student code must follow the FPTU format: 2 uppercase letters followed by 5 digits (e.g., SE19886, CE18793).";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Null/empty is allowed — use [Required] separately if mandatory
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var code = value.ToString()!;

            if (_regex.IsMatch(code))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(
                ErrorMessage,
                new[] { validationContext.MemberName ?? string.Empty }
            );
        }
    }
}
