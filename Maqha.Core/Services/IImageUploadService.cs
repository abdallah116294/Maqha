using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maqha.Core.Services
{
    public  interface IImageUploadService
    {
        //Upload image to the server and return the image URL
        Task<string> UploadImageAsync(IFormFile imageFile,string folder="images" );
        //Delete image from the server
        Task<bool> DeleteImageAsync(string imageUrl);
        //IsValidImage checks if the uploaded file is a valid image type
        bool IsValidImage(IFormFile imageFile);
    }
}
