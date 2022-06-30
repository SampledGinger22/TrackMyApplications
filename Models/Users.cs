#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HireMe.Models;

public class User 
{
    [Key]
    public int UserId { get;set; }

    [Required]
    [MinLength(2, ErrorMessage = "First name must be at least two characters in length")]
    public string FirstName { get;set; }

    [Required]
    [MinLength(2, ErrorMessage = "Last name must be at least two characters in length")]
    public string LastName { get;set; }

    [Required]
    [MinLength(3, ErrorMessage = "Username must be at least 3 characters long")]
    [MaxLength(15, ErrorMessage = "Username must be no longer than 15 characters long")]
    public string UserName { get;set; }

    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters in length.")]
    public string Password { get;set; }

    [NotMapped]
    [Compare("Password")]
    [DataType(DataType.Password)]
    public string Confirm { get;set; }

    public List<Application> Applications { get;set; } = new List<Application>();
    public DateTime Created_At { get;set; } = DateTime.Now;
    public DateTime Updated_At { get;set; } = DateTime.Now;
}