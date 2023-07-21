using System;

namespace Course.Services.Discount.Dtos
{
    public class DiscountUpdateDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int Rate { get; set; }
        public string Code { get; set; }
    }
}
