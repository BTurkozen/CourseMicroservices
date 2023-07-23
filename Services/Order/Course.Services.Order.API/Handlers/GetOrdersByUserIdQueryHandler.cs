using Course.Services.Order.API.Queries;
using Course.Services.Order.Application.Dtos;
using Course.Services.Order.Application.Mapping;
using Course.Services.Order.Infrastructure;
using Course.Shared.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Course.Services.Order.API.Handlers
{
    public class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, Response<List<OrderDto>>>
    {
        private readonly OrderDbContext _context;

        public GetOrdersByUserIdQueryHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Response<List<OrderDto>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await _context.Orders
                                       .Include(o => o.OrderItems)
                                       .Where(o => o.BuyerId == request.UserId)
                                       .ToListAsync();

            if (orders.Any() is false)
            {
                return Response<List<OrderDto>>.Success(new List<OrderDto>(), 200);
            }

            var ordersDto = ObjectMapper.Mapper.Map<List<OrderDto>>(orders);

            return Response<List<OrderDto>>.Success(ordersDto, 200);
        }
    }
}
