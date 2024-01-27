using Hospital_FinalP.Services.Abstract;
using System.IO;

namespace Hospital_FinalP.Services.Concrete
{
    public class FileService :IFileService
    {
        private readonly IWebHostEnvironment _env;

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }


        public string SaveImage(IFormFile imageFile)
        {
            try
            {
                var imgPath = _env.ContentRootPath;
                var path=Path.Combine(imgPath, "Images");

                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                //extensions
                FileInfo fileInfo = new(imageFile.FileName);
                var imageName = imageFile.FileName + Guid.NewGuid().ToString() + fileInfo.Extension;

                string fileNameWithPath = Path.Combine(path, imageName);

                var stream = new FileStream(fileNameWithPath, FileMode.Create);
                imageFile.CopyTo(stream);
                stream.Close();
                return (imageName);
            }
            catch 
            {
                return ("Error has occured!");
            }
        }


        public void DeleteFile(string fileName)
        {
            var imgPath = _env.ContentRootPath;
            var path = Path.Combine(imgPath, "Images");
            string filePath = Path.Combine(path, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

        }

    }
}
