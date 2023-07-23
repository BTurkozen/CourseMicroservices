﻿using Course.Services.Order.API.Commands;
using Course.Services.Order.Application.Dtos;
using Course.Services.Order.Domain.OrderAggregate;
using Course.Services.Order.Infrastructure;
using Course.Shared.Dtos;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Course.Services.Order.API.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response<CreatedOrderDto>>
    {
        private readonly OrderDbContext _context;

        public CreateOrderCommandHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Response<CreatedOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var newAddress = new Address(request.Address.Province, request.Address.District, request.Address.Street, request.Address.ZipCode, request.Address.Line);

            Domain.OrderAggregate.Order newOrder = new Domain.OrderAggregate.Order(request.BuyerId, newAddress);

            request.OrderItems.ForEach(oi =>
            {
                newOrder.AddOrderItem(oi.ProductId, oi.ProductName, oi.PictureUrl, oi.Price);
            });

            await _context.Orders.AddAsync(newOrder);

            var result = await _context.SaveChangesAsync();

            return Response<CreatedOrderDto>.Success(new CreatedOrderDto { OrderId = newOrder.Id }, 200);
        }
    }
}
