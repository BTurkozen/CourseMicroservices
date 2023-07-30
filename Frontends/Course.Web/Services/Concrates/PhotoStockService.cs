using Course.Web.Models.PhotoStockVMs;
using Course.Web.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Course.Web.Services.Concrates
{
    public class PhotoStockService : IPhotoStockService
    {
        private readonly HttpClient _httpClient;

        public PhotoStockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> DeletePhotoAsync(string photoUrl)
        {
            var response = await _httpClient.DeleteAsync($"photos?photoUrl={photoUrl}");

            return response.IsSuccessStatusCode;
        }

        public async Task<PhotoStockViewModel> UploadPhotoAsync(IFormFile photo)
        {
            if (photo is null || photo.Length <= 0)
            {
                return null;
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";

            using var stream = new MemoryStream();

            await photo.CopyToAsync(stream);

            var multiPartContent = new MultipartFormDataContent
            {
                { new ByteArrayContent(stream.ToArray()), "photoFile", fileName }
            };

            var response = await _httpClient.PostAsync("Photos", multiPartContent);

            if (response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<PhotoStockViewModel>();
        }
    }
}
