using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Services
{
    public interface IFileService
    {
        string GetImageUrl(string? imageName);
        Task<string?> ConvertToBase64(string filePath);
    }
}
