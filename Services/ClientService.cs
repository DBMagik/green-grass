using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenGrass.Models;

namespace GreenGrass.Services
{
    public interface IClientService
    {
        Task<List<Client>> GetAllClientsAsync();
        Task<Client?> GetClientByIdAsync(int id);
        Task<Client> CreateClientAsync(Client client);
        Task<Client> UpdateClientAsync(Client client);
        Task<bool> DeleteClientAsync(int id);
        Task<List<ServiceRecord>> GetClientServiceHistoryAsync(int clientId);
        Task<ServiceRecord> AddServiceRecordAsync(ServiceRecord serviceRecord);
        Task<ServiceRecord> UpdateServiceRecordAsync(ServiceRecord serviceRecord);
    }

    public class ClientService : IClientService
    {
        private readonly List<Client> _clients = [];
        private int _nextId = 1;
        private int _nextServiceId = 1;

        public ClientService()
        {
            // Add some sample data
            InitializeSampleData();
        }

        public Task<List<Client>> GetAllClientsAsync()
        {
            return Task.FromResult(_clients.Where(c => c.IsActive).ToList());
        }

        public Task<Client?> GetClientByIdAsync(int id)
        {
            var client = _clients.FirstOrDefault(c => c.Id == id);
            return Task.FromResult(client);
        }

        public Task<Client> CreateClientAsync(Client client)
        {
            client.Id = _nextId++;
            client.CreatedDate = DateTime.Now;
            _clients.Add(client);
            return Task.FromResult(client);
        }

        public Task<Client> UpdateClientAsync(Client client)
        {
            var existingClient = _clients.FirstOrDefault(c => c.Id == client.Id);
            if (existingClient != null)
            {
                var index = _clients.IndexOf(existingClient);
                _clients[index] = client;
            }
            return Task.FromResult(client);
        }

        public Task<bool> DeleteClientAsync(int id)
        {
            var client = _clients.FirstOrDefault(c => c.Id == id);
            if (client != null)
            {
                client.IsActive = false; // Soft delete
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<List<ServiceRecord>> GetClientServiceHistoryAsync(int clientId)
        {
            var client = _clients.FirstOrDefault(c => c.Id == clientId);
            return Task.FromResult(client?.ServiceHistory ?? []);
        }

        public Task<ServiceRecord> AddServiceRecordAsync(ServiceRecord serviceRecord)
        {
            serviceRecord.Id = _nextServiceId++;
            var client = _clients.FirstOrDefault(c => c.Id == serviceRecord.ClientId);
            if (client != null)
            {
                client.ServiceHistory.Add(serviceRecord);
            }
            return Task.FromResult(serviceRecord);
        }

        public Task<ServiceRecord> UpdateServiceRecordAsync(ServiceRecord serviceRecord)
        {
            var client = _clients.FirstOrDefault(c => c.Id == serviceRecord.ClientId);
            if (client != null)
            {
                var existingRecord = client.ServiceHistory.FirstOrDefault(s => s.Id == serviceRecord.Id);
                if (existingRecord != null)
                {
                    var index = client.ServiceHistory.IndexOf(existingRecord);
                    client.ServiceHistory[index] = serviceRecord;
                }
            }
            return Task.FromResult(serviceRecord);
        }

        private void InitializeSampleData()
        {
            _clients.Add(new Client
            {
                Id = _nextId++,
                FirstName = "John",
                LastName = "Smith",
                Email = "john.smith@email.com",
                PhoneNumber = "(555) 123-4567",
                Address = new Address
                {
                    Street = "123 Main St",
                    City = "Hometown",
                    State = "TX",
                    ZipCode = "12345"
                },
                PaymentMethodToken = "tok_visa1234",
                LastFourDigits = "1234",
                ServiceHistory = 
                [
                    new ServiceRecord
                    {
                        Id = _nextServiceId++,
                        ClientId = _nextId - 1,
                        ServiceType = ServiceType.LawnMowing,
                        Description = "Weekly lawn mowing service",
                        ServiceDate = DateTime.Now.AddDays(-7),
                        Amount = 50.00m,
                        Status = ServiceStatus.Completed,
                        CompletedDate = DateTime.Now.AddDays(-7),
                        PaymentStatus = PaymentStatus.Paid
                    }
                ]
            });
        }
    }
}