#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackMyApplication.Models;

public class Application
{
    [Key]
    public int ApplicationId { get;set; }

    public string? BusinessName { get;set; }

    public string? Location { get;set; }

    public string? JobTitle { get;set; }

    public string? Contact { get;set; }

    public string? CPhone { get;set; }

    [EmailAddress]
    public string? CEmail { get;set; }

    public string? CTitle { get;set; }

    public string? AddContacts { get;set; }

    public string? Pros { get;set; }

    public string? Cons { get;set; }

    public string? Notes { get;set; }

    public string? JobURL { get;set; }

    public bool Active { get;set; } = true;

    public string? MinSalary { get;set; }

    public string? MaxSalary { get;set; }

    public double? HourlyRate { get;set; }

    public DateTime? AppDate { get;set; }

    public int UserId { get;set; }

    public User? User { get;set; }

    public List<Interview> Interviews { get;set; } = new List<Interview>();

    public DateTime Created_At { get;set; } = DateTime.Now;
    public DateTime Updated_At { get;set; } = DateTime.Now;

}