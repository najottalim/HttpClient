using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using StudentApp.Console.Domain;
using StudentApp.Console.Service.Interfaces;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace StudentApp.Console.Service.Services;

public class StudentService : IStudentService
{
    public async Task<IEnumerable<Student>> GetAllAsync()
    {
        using (HttpClient client = new HttpClient())
        {
            return await client.GetFromJsonAsync<IEnumerable<Student>>(AppSettings.BASE_URL + "students");
        }
    }

    public async Task<Student> GetAsync(long id)
    {
        using (HttpClient client = new HttpClient())
        {
            return await client.GetFromJsonAsync<Student>(AppSettings.BASE_URL + "students/" + id);
        }
    }
    
    public async Task<Student> CreateAsync(StudentDto dto)
    {
        using (HttpClient client = new HttpClient())
        {
            string json = JsonSerializer.Serialize(dto);
            HttpResponseMessage response = await client.PostAsync(AppSettings.BASE_URL + "students", 
                new StringContent(json, Encoding.Default, "application/json"));
            
            string message = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(message))
                return null;
            
            return JsonConvert.DeserializeObject<Student>(message);
        }
    }

    public async Task<Student> UpdateAsync(long id, StudentDto dto)
    {
        using (HttpClient client = new HttpClient())
        {
            string json = JsonConvert.SerializeObject(dto);
            HttpResponseMessage response = await client.PutAsync(AppSettings.BASE_URL + "students/" + id, 
                new StringContent(json, Encoding.Default, "application/json"));
            
            string message = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(message))
                return null;
            
            return JsonConvert.DeserializeObject<Student>(message);
        }
    }

    public async Task<bool> DeleteAsync(long id)
    {
        using (HttpClient client = new HttpClient())
        {
            var result = await client.DeleteAsync(AppSettings.BASE_URL + "students/" + id);

            return result.IsSuccessStatusCode;
        }
    }

    public async Task UploadFilesAsync(long id, string imagePath, string passportPath)
    {
        using (HttpClient client = new HttpClient())
        {
            MultipartFormDataContent formData = new MultipartFormDataContent();
            formData.Add(new StreamContent(File.OpenRead(imagePath)), "image", "image.png");
            formData.Add(new StreamContent(File.OpenRead(passportPath)), "passport", "passport.png");
            
            HttpResponseMessage response = await client.PostAsync(AppSettings.BASE_URL + "students/attachments/" + id, formData);

            string message = await response.Content.ReadAsStringAsync();
        }
    }
}