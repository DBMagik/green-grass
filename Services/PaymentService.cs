
using System;
using System.Linq;
using System.Threading.Tasks;
using GreenGrass.Models;

namespace GreenGrass.Services
{
    public interface IPaymentService
    {
        Task<PaymentResult> ProcessPaymentAsync(Guid clientId, decimal amount, string description);
        Task<PaymentResult> ProcessPaymentAsync(PaymentMethod paymentMethod, decimal amount, string description);
        Task<bool> AddPaymentMethodAsync(Guid clientId, PaymentMethod paymentMethod);
        Task<bool> RemovePaymentMethodAsync(Guid clientId, int paymentMethodId);
    }

    public class PaymentService : IPaymentService
    {
        private readonly IClientService _clientService;

        public PaymentService(IClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task<PaymentResult> ProcessPaymentAsync(Guid clientId, decimal amount, string description)
        {
            var client = await _clientService.GetClientByIdAsync(clientId);
            if (client?.DefaultPaymentMethod == null)
            {
                return new PaymentResult { Success = false, ErrorMessage = "No default payment method found" };
            }

            return await ProcessPaymentAsync(client.DefaultPaymentMethod, amount, description);
        }

        public async Task<PaymentResult> ProcessPaymentAsync(PaymentMethod paymentMethod, decimal amount, string description)
        {
            // TODO: Integrate with actual payment processor (Stripe, Square, etc.)
            // This is a mock implementation
            
            await Task.Delay(1000); // Simulate API call

            // Mock success/failure (90% success rate)
            var random = new Random();
            var success = random.NextDouble() > 0.1;

            if (success)
            {
                return new PaymentResult
                {
                    Success = true,
                    TransactionId = Guid.NewGuid().ToString(),
                    Amount = amount,
                    ProcessedAt = DateTime.Now
                };
            }
            else
            {
                return new PaymentResult
                {
                    Success = false,
                    ErrorMessage = "Payment declined by card issuer"
                };
            }
        }

        public async Task<bool> AddPaymentMethodAsync(Guid clientId, PaymentMethod paymentMethod)
        {
            var client = await _clientService.GetClientByIdAsync(clientId);
            if (client == null) return false;

            paymentMethod.ClientId = clientId;
            paymentMethod.Id = client.PaymentMethods.Count + 1;
            
            // If this is the first payment method, make it default
            if (!client.PaymentMethods.Any())
            {
                paymentMethod.IsDefault = true;
            }

            client.PaymentMethods.Add(paymentMethod);
            await _clientService.UpdateClientAsync(client);
            return true;
        }

        public async Task<bool> RemovePaymentMethodAsync(Guid clientId, int paymentMethodId)
        {
            var client = await _clientService.GetClientByIdAsync(clientId);
            if (client == null) return false;

            var paymentMethod = client.PaymentMethods.FirstOrDefault(p => p.Id == paymentMethodId);
            if (paymentMethod == null) return false;

            client.PaymentMethods.Remove(paymentMethod);
            
            // If this was the default payment method, make another one default
            if (paymentMethod.IsDefault && client.PaymentMethods.Any())
            {
                client.PaymentMethods.First().IsDefault = true;
            }

            await _clientService.UpdateClientAsync(client);
            return true;
        }
    }
}