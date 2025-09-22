using MyLibrary.Services.Api;
using System.IO;
using System.Net.Http.Json;
using SixLabors.ImageSharp;
using MyLibrary.Resources.Languages;

namespace MyLibrary.Services
{
    public class FileService : IFileService
    {
        private readonly ApiService _apiService;
        private string _errors = string.Empty;

        public FileService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<string?> ConvertToBase64(string? filePath)
        {
            if(string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                _errors += Strings.FileService_Errors_InvalidFilePath + Environment.NewLine;
                _apiService.NotificationService.ShowError(title: Strings.CustomDialog_Title_Fail, message: _errors);
                return default;
            }

            if(!ValidateImageFile(filePath))
            {
                _apiService.NotificationService.ShowError(title: Strings.CustomDialog_Title_Fail, message: _errors);
                return default;
            }

            try
            {
                var bytes = await File.ReadAllBytesAsync(filePath);
                var base64string = Convert.ToBase64String(bytes);

                return base64string;
            }
            catch(Exception err)
            {
                _apiService.NotificationService.ShowError(title: Strings.CustomDialog_Title_Fail, message: err.Message);
                return default;
            }
        }

        public string GetImageUrl(string? imageName)
        {
            return _apiService.ApiSettings.BaseUrl + _apiService.ApiSettings.Images + 
                (!string.IsNullOrWhiteSpace(imageName) ?
                imageName : "default.png");
        }

        // Validates the image file for type and size.
        private bool ValidateImageFile(string filePath)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            long maxFileSizeInBytes = 5 * 1024 * 1024; // 5 MB
            string fileExtension;
            FileInfo fileInfo;

            try
            {
                fileExtension = System.IO.Path.GetExtension(filePath).ToLowerInvariant();
                fileInfo = new System.IO.FileInfo(filePath);
            }
            catch (Exception err)
            {
                _errors += Strings.FileService_Errors_InvalidFilePath + err.Message + Environment.NewLine;
                return false;
            }

            if (!allowedExtensions.Contains(fileExtension))
            {
                _errors += Strings.FileService_Errors_InvalidFileType + string.Join(", ", allowedExtensions) + Environment.NewLine;
            }

            if(fileInfo.Length > maxFileSizeInBytes)
            {
                _errors += Strings.FileService_Errors_InvalidFileSize + Environment.NewLine;
            }

            return string.IsNullOrEmpty(_errors);
        }
    }
}
