#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HireMe.Models;

public class Interview
{
    [Key]
    public int InterviewId { get;set; }

    [Required]
    public string? Title { get;set; } 

    [Required]
    public DateTime IntDate { get;set; }

    public string? Notes { get;set; }

    public int ApplicationId { get;set; }
    public Application? Application { get;set; }

    public DateTime Created_At { get;set; } = DateTime.Now;
    public DateTime Updated_At { get;set; } = DateTime.Now;

}