using System.ComponentModel.DataAnnotations;

public class UpdateProfileDto
{
    [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter a valid 10 digit phone number")]
    public string? PhoneNumber { get; set; }

    [StringLength(250)]
    public string? Address { get; set; }

}