using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace WasmTools1.StringAttributes;

public class PhilippinePhoneNumberAttr : ValidationAttribute
{
    protected override ValidationResult? IsValid(
              object?           value,
              ValidationContext validationContext)
    {
        if (value == null)
            return ValidationResult.Success;

        var input = value.ToString() ?? "";

        input = Regex.Replace(input, @"\D", "");

        if (input.StartsWith("09"))
        {
            input = string.Concat("63", input.AsSpan(1));
        }

        if (!Regex.IsMatch(input, @"^639\d{9}$"))
        {
            return new ValidationResult("Invalid Philippine phone number format. Must be +639XXXXXXXXX");
        }

        return ValidationResult.Success;
    }
}
