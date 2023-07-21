using Course.Services.Discount.Dtos;
using Course.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Course.Services.Discount.Services
{
    public interface IDiscountService
    {
        Task<Response<List<DiscountDto>>> GetAllAsync();
        Task<Response<DiscountDto>> GetByIdAsync();
        Task<Response<DiscountCreateDto>> SaveAsync(DiscountCreateDto discountCreateDto);
        Task<Response<DiscountUpdateDto>> UpdateAsync(DiscountUpdateDto discountUpdateDto);
        Task<Response<NoContent>> DeleteAsync(int id);
        Task<Response<DiscountDto>> GetByCodeAndUserId(string code, string userId);
    }
}
