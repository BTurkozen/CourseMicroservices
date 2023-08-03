using Course.Web.Models.PaymentVMs;
using System.Threading.Tasks;

namespace Course.Web.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> ReceivePayment(PaymentInfoInput paymentInfoInput);
    }
}
