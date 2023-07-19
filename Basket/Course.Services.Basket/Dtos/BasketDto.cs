using System.Collections.Generic;
using System.Linq;

namespace Course.Services.Basket.Dtos
{
    public sealed class BasketDto
    {
        public string UserId { get; set; }
        public string DiscountCode { get; set; }
        public List<BasketItemDto> BasketItems { get; set; }
        public decimal TotalPrice { get => BasketItems.Sum(bi => bi.Price * bi.Quantity); }
    }
}
