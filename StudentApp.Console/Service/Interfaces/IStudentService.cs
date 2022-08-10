using StudentApp.Console.Domain;

namespace StudentApp.Console.Service.Interfaces;

public interface IStudentService
{
    Task<IEnumerable<Student>> GetAllAsync(string login, string password);
    Task<Student> GetAsync(long id);
    Task<Student> CreateAsync(StudentDto dto);
    Task<Student> UpdateAsync(long id, StudentDto dto);
    Task<bool> DeleteAsync(long id);
    Task UploadFilesAsync(long id, string imagePath, string passportPath);
}