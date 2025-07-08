using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenGrass.Models;
using GreenGrass.Data;
using Microsoft.EntityFrameworkCore;

namespace GreenGrass.Services
{
    public interface IClientService
    {
        Task<List<Client>> GetAllClientsAsync();
        Task<Client?> GetClientByIdAsync(Guid id);
        Task<Client> CreateClientAsync(Client client);
        Task<Client> UpdateClientAsync(Client client);
        Task<bool> DeleteClientAsync(Guid id);
        Task<List<ServiceRecord>> GetClientServiceHistoryAsync(Guid clientId);
        Task<ServiceRecord> AddServiceRecordAsync(ServiceRecord serviceRecord);
        Task<ServiceRecord> UpdateServiceRecordAsync(ServiceRecord serviceRecord);
    }

    public class ClientService : IClientService
    {
        private readonly ApplicationDbContext _context;

        public ClientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Client>> GetAllClientsAsync()
        {
            try
            {
                var customers = await _context.Customers.Where(c => c.IsActive).ToListAsync();
                return customers.Select(MapToClient).ToList();  // Map DB entity to model
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving clients: {ex.Message}");
                throw new ApplicationException("Failed to retrieve clients from database.", ex);
            }
        }

        public async Task<Client?> GetClientByIdAsync(Guid id)
        {
            try
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
                return customer != null ? MapToClient(customer) : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving client {id}: {ex.Message}");
                throw new ApplicationException($"Failed to retrieve client {id} from database.", ex);
            }
        }

        public async Task<Client> CreateClientAsync(Client client)
        {
            try
            {
                var customer = MapToCustomer(client);
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                client.Id = customer.Id;  // Update model with DB-generated ID
                return client;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating client: {ex.Message}");
                throw new ApplicationException("Failed to create client in database.", ex);
            }
        }

        public async Task<Client> UpdateClientAsync(Client client)
        {
            try
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == client.Id);
                if (customer == null)
                {
                    throw new KeyNotFoundException($"Client {client.Id} not found.");
                }
                MapToCustomer(client, customer);  // Update existing entity
                await _context.SaveChangesAsync();
                return client;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating client {client.Id}: {ex.Message}");
                throw new ApplicationException($"Failed to update client {client.Id} in database.", ex);
            }
        }

        public async Task<bool> DeleteClientAsync(Guid id)
        {
            try
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
                if (customer != null)
                {
                    customer.IsActive = false;  // Soft delete
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting client {id}: {ex.Message}");
                throw new ApplicationException($"Failed to delete client {id} from database.", ex);
            }
        }

        // In a full implementation, add related entities/tables
        public Task<List<ServiceRecord>> GetClientServiceHistoryAsync(Guid clientId)
        {
            // TODO: Implement DB support once ServiceRecord table is defined
            return Task.FromResult(new List<ServiceRecord>());  // Placeholder to avoid breaking
        }

        public Task<ServiceRecord> AddServiceRecordAsync(ServiceRecord serviceRecord)
        {
            // TODO: Implement DB support
            throw new NotImplementedException("Service history DB operations not supported yet.");
        }

        public Task<ServiceRecord> UpdateServiceRecordAsync(ServiceRecord serviceRecord)
        {
            // TODO: Implement DB support
            throw new NotImplementedException("Service history DB operations not supported yet.");
        }
private Client MapToClient(Customer customer)
        {
            return new Client
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Address = new Address
                {
                    Street = customer.Street,
                    City = customer.City,
                    State = customer.State,
                    ZipCode = customer.ZipCode
                    // Note: Country is in DB but not in Address; ignored here
                },
                CreatedDate = customer.CreatedDate,
                IsActive = customer.IsActive,
                PaymentMethodToken = customer.PaymentMethodToken,
                LastFourDigits = customer.LastFourDigits,
                // ServiceHistory and PaymentMethods: Not mapped as no DB support
                ServiceHistory = [],  // Placeholder
                PaymentMethods = []   // Placeholder
            };
        }

        private Customer MapToCustomer(Client client, Customer? existing = null)
        {
            var customer = existing ?? new Customer();
            customer.Id = client.Id;  // Only for updates
            customer.FirstName = client.FirstName;
            customer.LastName = client.LastName;
            customer.Email = client.Email;
            customer.PhoneNumber = client.PhoneNumber;
            customer.Street = client.Address?.Street ?? string.Empty;
            customer.City = client.Address?.City ?? string.Empty;
            customer.State = client.Address?.State ?? string.Empty;
            customer.ZipCode = client.Address?.ZipCode ?? string.Empty;
            customer.Country = "USA";  // Default; adjust as needed since not in model/UI
            customer.CreatedDate = client.CreatedDate;
            customer.IsActive = client.IsActive;
            customer.PaymentMethodToken = client.PaymentMethodToken;
            customer.LastFourDigits = client.LastFourDigits;
            return customer;
        }
    }
}