using Microsoft.EntityFrameworkCore;
using StudentApp.Api.Models;

namespace StudentApp.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options)
    {
    }
    
    public virtual DbSet<Student> Students { get; set; }
    public virtual DbSet<Attachment> Attachments { get; set; }

}