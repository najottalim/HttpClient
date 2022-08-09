namespace StudentApp.Api.Service;

public class HelperService
{
    private readonly IWebHostEnvironment env;

    public HelperService(IWebHostEnvironment env)
    {
        this.env = env;
    }
    
    public async Task<(string name, string path)> SaveFileAsync(IFormFile file)
    {
        string path = Path.Combine(env.WebRootPath, "Images");
        string name = Guid.NewGuid().ToString("N") + ".png";
        string filePath = Path.Combine(path, name);
        
        FileStream fileStream = System.IO.File.Create(filePath);
        await file.CopyToAsync(fileStream);
        
        await fileStream.FlushAsync();
        fileStream.Close();

        return (name, Path.Combine("Images", name));
    }
}