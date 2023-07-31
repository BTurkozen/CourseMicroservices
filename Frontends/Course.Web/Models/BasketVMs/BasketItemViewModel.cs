namespace Course.Web.Models.BasketVMs
{
    public class BasketItemViewModel
    {
        public int Quantity { get; set; } = 1;
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public decimal Price { get; set; }

        private decimal? DiscountAppliedPrice { get; set; }

        public decimal GetCurrentPice { get => DiscountAppliedPrice != null ? DiscountAppliedPrice.Value : Price; }

        public void AppliedDicount(decimal disCountPrice)
        {
            DiscountAppliedPrice = disCountPrice;
        }
    }
}
