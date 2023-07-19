using Course.Service.PhotoStock.Dtos;
using Course.Shared.ControllerBases;
using Course.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Course.Services.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : CustomBaseController
    {
        [HttpGet]
        public async Task<IActionResult> PhotoSave(IFormFile photoFile, CancellationToken cancellationToken)
        {
            if (photoFile is not null && photoFile.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Photos", photoFile.FileName);

                using var stream = new FileStream(path, FileMode.Create);

                await photoFile.CopyToAsync(stream, cancellationToken);

                var returnPath = $"Photos/{photoFile.FileName}";

                PhotoDto photoDto = new() { PhotoURL = returnPath };

                return CreateActionResultInstance(Response<PhotoDto>.Success(photoDto, 200));
            }

            return CreateActionResultInstance(Response<PhotoDto>.Fail("Photo is Empty", 400));
        }

        public async Task<IActionResult> PhotoDelete(string photoURL)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Photos", photoURL);

            if (System.IO.File.Exists(path) is false)
            {
                return CreateActionResultInstance(Response<NoContent>.Fail("Photo not found", 404));
            }

            System.IO.File.Delete(path);

            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }
    }
}
