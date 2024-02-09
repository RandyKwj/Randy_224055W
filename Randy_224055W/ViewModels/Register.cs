using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

using System;
using Microsoft.AspNetCore.Identity;

namespace Randy_224055W.ViewModels
{
    public class Register : IdentityUser
    {


        [Required]
        public string Name{ get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]{9}$", ErrorMessage = "NRIC must be 9 characters (alphanumeric)")]
        public string NRIC { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9\s\.,!@#\$%\^&\*\(\)-_+=\[\]\{\};:'""<>\?/\\|`~]*$", ErrorMessage = "Special characters are allowed")]
        public string WhoAmI { get; set; }

        public string? Resume { get; set; }
    }
}
