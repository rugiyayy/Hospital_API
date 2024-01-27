namespace Hospital_FinalP.Services.Abstract
{
    public interface IFileService
    {
        public string SaveImage(IFormFile imageFile);
        public void DeleteFile(string fileName);


    }
}
