using System.Collections.Generic;
using System;

namespace Course.Web.Models.OrderVMs
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }

        // Ödeme Geçmişinde Adres alanına gerek olmadığından dolayı alınmadı.
        //public AddressDto Address { get; set; }

        public string BuyerId { get; set; }

        public List<OrderItemViewModel> OrderItems { get; set; }
    }
}
