using Course.Shared.Dtos;
using Course.Web.Models;
using Course.Web.Models.CatalogVMs;
using Course.Web.Services.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Course.Web.Services.Concrates
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;
        private readonly IPhotoStockService _photoStockService;

        public CatalogService(HttpClient httpClient, IPhotoStockService photoStockService)
        {
            _httpClient = httpClient;
            _photoStockService = photoStockService;
        }

        public async Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput)
        {
            var resultPhoto = await _photoStockService.UploadPhotoAsync(courseCreateInput.PhotoFormFile);

            if (resultPhoto is not null)
            {
                courseCreateInput.Picture = resultPhoto.PhotoURL;
            }

            var response = await _httpClient.PostAsJsonAsync<CourseCreateInput>("courses", courseCreateInput);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCourseAsync(string courseId)
        {
            var response = await _httpClient.DeleteAsync($"courses/{courseId}");

            return response.IsSuccessStatusCode;
        }

        public async Task<List<CategoryViewModel>> GetAllCategoryAsync()
        {
            var response = await _httpClient.GetAsync("categories");

            if (response.IsSuccessStatusCode is false)
            {
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<Response<List<CategoryViewModel>>>();

            return result.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourseAsync()
        {
            // http://localhost:5000/services/catalog/courses
            var response = await _httpClient.GetAsync("courses");

            if (response.IsSuccessStatusCode is false)
            {
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();

            return result.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"courses/getalluserid/{userId}");

            if (response.IsSuccessStatusCode is false)
            {
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();

            return result.Data;
        }

        public async Task<CourseViewModel> GetByCourseIdAsync(string courseId)
        {
            var response = await _httpClient.GetAsync($"courses/{courseId}");

            if (response.IsSuccessStatusCode is false)
            {
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<Response<CourseViewModel>>();

            return result.Data;
        }

        public async Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput)
        {
            var response = await _httpClient.PutAsJsonAsync<CourseUpdateInput>("courses", courseUpdateInput);

            return response.IsSuccessStatusCode;
        }
    }
}
