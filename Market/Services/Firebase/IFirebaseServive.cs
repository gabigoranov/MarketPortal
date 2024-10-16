﻿namespace Market.Services.Firebase
{
    public interface IFirebaseServive
    {
        public Task UploadFileAsync(IFormFile file, string folderName, string fileName);
        public Task DeleteFileAsync(string folderName, string fileName);

        public Task<IFormFile> GetFileAsync(string folderName, string fileName);

    }
}
