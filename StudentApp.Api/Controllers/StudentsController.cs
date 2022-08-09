using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApp.Api.Data;
using StudentApp.Api.Models;
using StudentApp.Api.Models.DTOs;
using StudentApp.Api.Service;

namespace StudentApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly AppDbContext dbContext;
    private readonly IWebHostEnvironment env;
    public StudentsController(AppDbContext dbContext, IWebHostEnvironment env)
    {
        this.dbContext = dbContext;
        this.env = env;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetStudents()
    {
        var students = await dbContext.Students
            .Include(p => p.Image)
            .Include(p => p.Passport)
            .ToListAsync();
        
        return Ok(students);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetStudent(long id)
    {
        var student = await dbContext.Students
            .Include(p => p.Image)
            .Include(p => p.Passport)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (student == null)
            return NotFound(student);
        
        return Ok(student);
    }

    [HttpPost]
    public async Task<IActionResult> PostStudent(StudentDto studentDto)
    {
        var student = new Student()
        {
            FirstName = studentDto.FirstName,
            LastName = studentDto.LastName,
            Faculty = studentDto.Faculty
        };

        var entry = await dbContext.Students.AddAsync(student);
        await dbContext.SaveChangesAsync();

        return Ok(entry.Entity);
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateStudent(long id, StudentDto studentDto)
    {
        var student = await dbContext.Students.FirstOrDefaultAsync(p => p.Id == id);

        if (student is null)
        {
            return NotFound("yoq xech narsa");
        }
        
        student.FirstName = studentDto.FirstName;
        student.LastName = studentDto.LastName;
        student.Faculty = studentDto.Faculty;
        student.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();

        return Ok(student);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteStudent(long id)
    {
        var student = await dbContext.Students.FirstOrDefaultAsync(p => p.Id == id);
        if (student is null)
        {
            return NotFound("Yooq");
        }

        dbContext.Students.Remove(student);
        await dbContext.SaveChangesAsync();
        
        return Ok("Deleted");
    }

    [HttpPatch]
    [Route("{id}")]
    public async Task<IActionResult> UpdateFaculty(long id, string faculty)
    {
        var student = await dbContext.Students.FirstOrDefaultAsync(p => p.Id == id);
        if (student is null)
        {
            return NotFound("Yoooq");
        }

        student.Faculty = faculty;
        student.UpdatedAt = DateTime.UtcNow;
        
        await dbContext.SaveChangesAsync();

        return Ok(student);
    }

    [HttpPost]
    [Route("Attachments/{id}")]
    public async Task<IActionResult> PostAttachments(long id, [FromForm]StudentFile files)
    {
        HelperService helper = new HelperService(env);
        // student
        var student = await dbContext.Students.FirstOrDefaultAsync(p => p.Id == id);
        if (student is null)
        {
            return NotFound("Student yo");
        }

        var imageDetail = await helper.SaveFileAsync(files.Image);
        var passportDetail = await helper.SaveFileAsync(files.Passport);
        // student
        var entry = await dbContext.Attachments.AddAsync(new Attachment()
        {
            Name = imageDetail.name,
            Path = imageDetail.path
        });
        var passportEntry = await dbContext.Attachments.AddAsync(new Attachment()
        {
            Name = passportDetail.name,
            Path = passportDetail.path
        });
        
        await dbContext.SaveChangesAsync();
        
        student.PassportId = passportEntry.Entity.Id;
        student.ImageId = entry.Entity.Id;
        
        await dbContext.SaveChangesAsync();

        return Ok("saved");
    }
}