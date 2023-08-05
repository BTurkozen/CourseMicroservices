using Course.Services.FakePayment.Dtos;
using Course.Shared.ControllerBases;
using Course.Shared.Dtos;
using Course.Shared.Messages;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Course.Services.FakePayment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentsController : CustomBaseController
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public FakePaymentsController(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost]
        public async Task<IActionResult> ReceviePayment(PaymentInfoDto paymentInfoDto)
        {
            // Bir Endpoint ver diyoruz.
            // içerisine hangi kuyruğa gönderileceğini tanımlıyoruz.
            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-order-service"));

            var createOrderMessageCommand = new CreateOrderMessageCommand()
            {
                BuyerId = paymentInfoDto.Order.BuyerId,
                Province = paymentInfoDto.Order.Address.Province,
                District = paymentInfoDto.Order.Address.District,
                Line = paymentInfoDto.Order.Address.Line,
                Street = paymentInfoDto.Order.Address.Street,
                ZipCode = paymentInfoDto.Order.Address.ZipCode,
            };

            paymentInfoDto.Order.OrderItems.ForEach(oi =>
            {
                createOrderMessageCommand.OrderItems.Add(new OrderItem
                {
                    PictureUrl = oi.PictureUrl,
                    Price = oi.Price,
                    ProductId = oi.ProductId,
                    ProductName = oi.ProductName,
                });
            });

            await sendEndpoint.Send(createOrderMessageCommand);

            return CreateActionResultInstance(Shared.Dtos.Response<NoContentResult>.Success(200));
        }
    }
}
