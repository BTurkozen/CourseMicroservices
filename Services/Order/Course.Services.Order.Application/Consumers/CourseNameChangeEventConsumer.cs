using Course.Services.Order.Infrastructure;
using Course.Shared.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Services.Order.Application.Consumers
{
    public class CourseNameChangeEventConsumer : IConsumer<CourseNameChangedEvent>
    {
        private readonly OrderDbContext _orderDbContext;

        public CourseNameChangeEventConsumer(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task Consume(ConsumeContext<CourseNameChangedEvent> context)
        {
            var orderItems = await _orderDbContext.OrderItems
                                                  .Where(oi => oi.ProductId == context.Message.CourseId)
                                                  .ToListAsync();

            orderItems.ForEach(oi =>
            {
                oi.UpdateOrderItem(context.Message.UpdatedName, oi.PictureUrl, oi.Price);
            });

            await _orderDbContext.SaveChangesAsync();
        }
    }
}
