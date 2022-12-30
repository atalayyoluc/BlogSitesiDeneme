using Blog.Entity.DTOs.Images;
using Blog.Entity.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service.Helpers.Images
{
    public class ImageHelper : IImageHelper
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly string wwroot;
        private const string imgFolder = "images";
        private const string articleImagesFolder = "Article-img";
        private const string userImageFolder = "User-img";
        public ImageHelper(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
            wwroot = webHostEnvironment.WebRootPath;
        }
        private string ReplaceInvalidChars(string fileName) {
            return fileName.Replace("İ", "I")
                    .Replace("ı", "i")
                    .Replace("Ğ", "G")
                    .Replace("ğ", "g")
                    .Replace("Ü", "U")
                    .Replace("ü", "u")
                    .Replace("ş", "s")
                    .Replace("Ş", "S")
                    .Replace("Ö", "O")
                    .Replace("ö", "o")
                    .Replace("Ç", "C")
                    .Replace("ç", "c")
                    .Replace("é", "")
                    .Replace("!", "")
                    .Replace("'", "")
                    .Replace("^", "")
                    .Replace("+", "")
                    .Replace("%", "")
                    .Replace("/", "")
                    .Replace("(", "")
                    .Replace(")", "")
                    .Replace("=", "")
                    .Replace("?", "")
                    .Replace("_", "")
                    .Replace("*", "")
                    .Replace("æ", "")
                    .Replace("ß", "")
                    .Replace("@", "")
                    .Replace("€", "")
                    .Replace("<", "")
                    .Replace(">", "")
                    .Replace("#", "")
                    .Replace("$", "")
                    .Replace("½", "")
                    .Replace("{", "")
                    .Replace("[", "")
                    .Replace("]", "")
                    .Replace("}", "")
                    .Replace(@"\", "")
                    .Replace("|", "")
                    .Replace("~", "")
                    .Replace("¨", "")
                    .Replace(",", "")
                    .Replace(";", "")
                    .Replace("`", "")
                    .Replace(".", "")
                    .Replace(":", "")
                    .Replace(" ", "");

        }

        public async Task<ImageUploadDTO> Upload(string name, IFormFile imageFile,ImageType imageType, string folderName = null)
        {
            folderName ??= imageType == ImageType.User ? userImageFolder : articleImagesFolder;
            if(!Directory.Exists($"{wwroot}/{imgFolder}/{folderName}"))
                Directory.CreateDirectory($"{wwroot}/{imgFolder}/{folderName}");

            string oldFileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
            string fileExtension= Path.GetExtension(imageFile.FileName);

            name = ReplaceInvalidChars(name);
            DateTime dateTime=DateTime.Now;

            string newFileName = $"{name}_{dateTime.Millisecond}{fileExtension}";
            var path = Path.Combine($"{wwroot}/{imgFolder}/{folderName}",newFileName);

            await using var stream =new FileStream(path,FileMode.Create,FileAccess.Write,FileShare.None,1024*1024,useAsync:false);
            await imageFile.CopyToAsync(stream);
            await stream.FlushAsync();
            string message = imageType == ImageType.User ? $"{newFileName} isimli kullanıcı resmi başarıyla eklenmiştir" : $"{newFileName} isimli makale resmi başarıyla eklenmiştir";
            return new ImageUploadDTO()
            {
                FullName = $"{folderName}/{newFileName}"
            };
        }
        public void Delete(string imageName)
        {
            var fileToDelete = Path.Combine($"{wwroot}/{imgFolder}/{imageName}");
            if (File.Exists(fileToDelete))
            {
                File.Delete(fileToDelete);
            }
        }

    }
}
