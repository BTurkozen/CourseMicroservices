using System;
using System.Collections.Generic;
using System.Linq;

namespace Course.Web.Models.BasketVMs
{
    public class BasketViewModel
    {
        public BasketViewModel()
        {
            _basketItems = new List<BasketItemViewModel>();
        }
        public string UserId { get; set; }
        public string DiscountCode { get; set; }
        public int? DiscountRate { get; set; }
        private List<BasketItemViewModel> _basketItems { get; set; }

        public List<BasketItemViewModel> BasketItems
        {
            get
            {
                if (HasDiscount)
                {
                    _basketItems.ForEach(bi =>
                    {
                        var discountPrice = bi.Price * ((decimal)DiscountRate.Value / 100);
                        bi.AppliedDicount(Math.Round(bi.Price - discountPrice, 2));
                    });
                }
                return _basketItems;
            }
            set { _basketItems = value; }
        }
        public decimal TotalPrice { get => _basketItems.Sum(bi => bi.GetCurrentPice * bi.Quantity); }

        public bool HasDiscount
        {
            get => !string.IsNullOrEmpty(DiscountCode) && DiscountRate.HasValue;
        }
    }
}
