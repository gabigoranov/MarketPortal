namespace Market.Services.Firebase
{
    public interface IFirebaseServive
    {
        public Task UploadFileAsync(IFormFile file, string folderName, string fileName);
        public Task DeleteFileAsync(string folderName, string fileName);
        public Task<string> GetImageUrl(string path, string imageId);

        public Task<IFormFile> GetFileAsync(string folderName, string fileName);
        public void SaveFile(IFormFile file, string name);

    }
}
