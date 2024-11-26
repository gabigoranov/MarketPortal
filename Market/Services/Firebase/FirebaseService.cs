
using Firebase.Auth;
using Firebase.Storage;
using System.Drawing.Imaging;
using System.Drawing;

namespace Market.Services.Firebase
{
    public class FirebaseService : IFirebaseServive
    {
        private static string ApiKey = "AIzaSyC4NuBfxIl3AWAwTLXqWhJAdvm14iIn12I";
        private static string Bucket = "market-229ca.appspot.com";

        private FirebaseAuthProvider auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
        private FirebaseStorage storage;

        public FirebaseService()
        {
            Setup();
        }

        private async void Setup()
        {
            this.storage = new FirebaseStorage(
                Bucket,
                new FirebaseStorageOptions
                {
                    ThrowOnCancel = true
                });
        }

        public async Task DeleteFileAsync(string folderName, string fileName)
        {
            await storage.Child(folderName).Child(fileName).DeleteAsync();
        }

        public void SaveFile(IFormFile file, string name)
        {
            using (var inputStream = file.OpenReadStream())
            {
                using (var image = Image.FromStream(inputStream))
                {
                    using (var outputStream = new MemoryStream())
                    {
                        image.Save(outputStream, ImageFormat.Jpeg);

                        System.IO.File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\", name), outputStream.ToArray());
                    }
                }
            }
        }

        public async Task UploadFileAsync(IFormFile file, string folderName, string fileName)
        {
            var stream = file.OpenReadStream();
            var cancellation = new CancellationTokenSource();
            await storage.Child(folderName)
                .Child($"{fileName}")
                .PutAsync(stream, cancellation.Token);
        }

        public async Task<IFormFile> GetFileAsync(string folderName, string fileName)
        {
            string url = await storage.Child(folderName).Child(fileName).GetDownloadUrlAsync();
            using (HttpClient httpClient = new HttpClient())
            {
                var fileData = await httpClient.GetByteArrayAsync(url);
                var stream = new MemoryStream(fileData);

                // Creating a FormFile instance from the stream
                IFormFile formFile = new FormFile(stream, 0, stream.Length, "offer", fileName)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "application/octet-stream"
                };

                return formFile;
            }
        }
    }
}
