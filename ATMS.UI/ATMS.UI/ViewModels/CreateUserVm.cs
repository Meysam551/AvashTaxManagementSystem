namespace ATMS.UI.ViewModels;

using System.ComponentModel.DataAnnotations;

public class CreateUserVm
{
    [Required]
    [Display(Name = "نام کاربری")]
    public string Username { get; set; } = "";

    [Required]
    [EmailAddress]
    [Display(Name = "ایمیل")]
    public string Email { get; set; } = "";

    [Required]
    [Display(Name = "نام")]
    public string FirstName { get; set; } = "";

    [Required]
    [Display(Name = "نام خانوادگی")]
    public string LastName { get; set; } = "";

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "کلمه عبور")]
    public string Password { get; set; } = "";
}


