using System.ComponentModel.DataAnnotations.Schema;
using StudentApp.Api.Models.Commons;

namespace StudentApp.Api.Models;

public class Student : Auditable
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Faculty { get; set; }
    
    public long? PassportId { get; set; }
    
    [ForeignKey(nameof(PassportId))]
    public Attachment? Passport { get; set; }
    
    public long? ImageId { get; set; }
    
    [ForeignKey(nameof(ImageId))]
    public Attachment? Image { get; set; }
}