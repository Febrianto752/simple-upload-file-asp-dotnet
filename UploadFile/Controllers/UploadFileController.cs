using Microsoft.AspNetCore.Mvc;

namespace UploadFile.Controllers;

public class UploadFileController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost("UploadFile")]
    public async Task<IActionResult> Index(List<IFormFile> files)
    {

        Console.WriteLine($"files : {files}"); // output : System.Collections.Generic.List`1[Microsoft.AspNetCore.Http.IFormFile]
        var size = files.Sum(h => h.Length);
        var filePaths = new List<string>();

        Console.WriteLine($"size files : {size}");


        foreach (var file in files)
        {
            Console.WriteLine($"file : {file}"); // output : Microsoft.AspNetCore.Http.FormFile

            Console.WriteLine($" name : {file.Name}"); // output : file
            Console.WriteLine($" filename : {file.FileName}"); // output : gambar.jpg
            Console.WriteLine($" length : {file.Length}"); // output : 198383 = 194kb
            Console.WriteLine($" content type : {file.ContentType}"); // output : image/jpeg
            Console.WriteLine($" headers : {file.Headers}"); // output : Microsoft.AspNetCore.Http.HeaderDictionary
            Console.WriteLine($" get current direktori : {Directory.GetCurrentDirectory()}"); // output : C:\Users\Febrianto\Desktop\latihan-upload-file\UploadFile\UploadFile
            Console.WriteLine($" get extension file : {Path.GetExtension(file.FileName)}"); // output : .jpg


            if (file.Length > 0)
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images");

                if (!Directory.Exists(folderPath))
                {
                    try
                    {
                        Directory.CreateDirectory(folderPath);
                        Console.WriteLine("Folder created successfully!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to create folder: " + ex.Message);

                    }
                }
                else
                {
                    Console.WriteLine("Folder already exists.");
                }
                var randomName = GenerateRandomString();
                var urlImage = $"images/{randomName}/{file.FileName}";
                Console.WriteLine($"url image : {urlImage}");

                var filePath = $"{folderPath}\\{randomName}-{file.FileName}";

                filePaths.Add(filePath);

                Console.WriteLine($"file Path : {filePath}");
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
        }

        return Ok(new { files.Count, size, filePaths });
    }

    public string GenerateRandomString(int length = 20)
    {
        var random = new Random();
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)])
            .ToArray());
    }

}

