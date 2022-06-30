using Microsoft.EntityFrameworkCore;
namespace TrackMyApplication.Models;
// the MyContext class representing a session with our MySQL 
// database allowing us to query for or save data
public class MyContext : DbContext 
{ 
    public MyContext(DbContextOptions options) : base(options) { }
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Application> Applications { get;set; } = null!;
    public DbSet <Interview> Interviews { get;set; } = null!;

}