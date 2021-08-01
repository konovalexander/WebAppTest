using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Logic.ValidationAttributes
{
    public class PhoneNumberAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            return Regex.IsMatch((string)value, @"^7\d{10}$");
        }
    }
}
