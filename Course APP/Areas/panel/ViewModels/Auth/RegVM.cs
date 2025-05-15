using System.ComponentModel.DataAnnotations;

namespace Course_APP.Areas.panel.ViewModels.Auth
{
    public class RegVM
    {

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Surname { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; } = false;

        [Required]
        public string Role { get; set; } = "Teacher";
    }

}
