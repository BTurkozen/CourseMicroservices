using Course.Services.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Services.Order.Domain.OrderAggregate
{
    // EFCORE Features
    // - Owned Types
    // - Shadow Property
    // - Backing Field
    // İsimlendirmeleri ürün olarak genel bakılarak verilmesi ileride Kurs değilde başka bir ürün satılması durumuna göre de düşünülerek yapılması.
    public class Order : Entity, IAggregateRoot
    {
        public DateTime CreatedDate { get; private set; }

        // Owned Entity Types
        public Address Address { get; private set; }

        public string BuyerId { get; private set; }

        // EFCore içerisinde property'den ziyade field ile yapıyorsanız. Backing Field olarak geçmektedir.
        // Encapsulate etmeye yaramaktadır.
        // Yapılmasının sebebi Data ekleneceği zaman bağlı bulunduğu Entity / IAggregateRoot üzerinden eklensin.
        private readonly List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        public Order(Address address, string buyerId)
        {
            _orderItems = new List<OrderItem>();
            CreatedDate = DateTime.Now;
            Address = address;
            BuyerId = buyerId;
        }

        /// <summary>
        /// Order Item eklemesi için bir method tanımlıyoruz.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productName"></param>
        /// <param name="pictureUrl"></param>
        /// <param name="price"></param>
        public void AddOrderItem(string productId, string productName, string pictureUrl, decimal price)
        {
            // ProductId'ye ait bir ürün varmı onu kontrol ediyoruz.
            var existProduct = _orderItems.Any(oi => oi.ProductId == productId);

            if (existProduct is false)
            {
                var newOrderItem = new OrderItem(productId, productName, pictureUrl, price);
                
                _orderItems.Add(newOrderItem);
            }
        }

        /// <summary>
        /// Sipariş için Toplam datayı dönüyoruz.
        /// </summary>
        public decimal GetTotalPrice=> _orderItems.Sum(oi => oi.Price);
    }
}
