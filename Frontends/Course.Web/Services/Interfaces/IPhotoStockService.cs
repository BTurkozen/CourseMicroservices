using Course.Web.Models.PhotoStockVMs;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Course.Web.Services.Interfaces
{
    public interface IPhotoStockService
    {
        Task<PhotoStockViewModel> UploadPhotoAsync(IFormFile photo);
        Task<bool> DeletePhotoAsync(string photoUrl);
    }
}
