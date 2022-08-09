using StudentApp.Api.Models.Commons;

namespace StudentApp.Api.Models;

public class Attachment : Auditable
{
    public string Name { get; set; }
    public string Path { get; set; }
}