using System.ComponentModel.DataAnnotations;

namespace AuthTest.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "email is not specified")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Enter email")]
    public string? Email { get; set; }
    [Required(ErrorMessage = "password is not specified")]
    [DataType(DataType.Password)]
    [Display(Name = "Enter password")]
    public string? Password { get; set; }
}