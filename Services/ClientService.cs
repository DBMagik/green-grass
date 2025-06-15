// Services/ClientService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GreenGrass.Models;
using GreenGrass.Data;


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
        private readonly ApplicationDbContext _context;


        public ClientService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<Client>> GetAllClientsAsync()
        {
            try
            {
                var customers = await _context.Customers
                    .Where(c => c.IsActive)
                    .OrderBy(c => c.LastName)
                    .ThenBy(c => c.FirstName)
                    .ToListAsync();


                return customers.Select(MapCustomerToClient).ToList();
            }
            catch (Exception ex)
            {
                // Log the exception in a real application
                throw new InvalidOperationException("Failed to retrieve clients from database", ex);
            }
        }


        public async Task<Client?> GetClientByIdAsync(int id)
        {
            try
            {
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Id == id);


                return customer != null ? MapCustomerToClient(customer) : null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve client with ID {id}", ex);
            }
        }


        public async Task<Client> CreateClientAsync(Client client)
        {
            try
            {
                var customer = MapClientToCustomer(client);
                customer.CreatedDate = DateTime.Now;


                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();


                return MapCustomerToClient(customer);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create client", ex);
            }
        }


        public async Task<Client> UpdateClientAsync(Client client)
        {
            try
            {
                var existingCustomer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Id == client.Id);


                if (existingCustomer == null)
                {
                    throw new InvalidOperationException($"Client with ID {client.Id} not found");
                }


                // Update properties
                existingCustomer.FirstName = client.FirstName;
                existingCustomer.LastName = client.LastName;
                existingCustomer.Email = client.Email;
                existingCustomer.PhoneNumber = client.PhoneNumber;
                existingCustomer.Street = client.Address.Street;
                existingCustomer.City = client.Address.City;
                existingCustomer.State = client.Address.State;
                existingCustomer.ZipCode = client.Address.ZipCode;
                existingCustomer.PaymentMethodToken = client.PaymentMethodToken;
                existingCustomer.LastFourDigits = client.LastFourDigits;
                existingCustomer.IsActive = client.IsActive;


                await _context.SaveChangesAsync();


                return MapCustomerToClient(existingCustomer);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to update client with ID {client.Id}", ex);
            }
        }


        public async Task<bool> DeleteClientAsync(int id)
        {
            try
            {
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Id == id);


                if (customer == null)
                {
                    return false;
                }


                customer.IsActive = false; // Soft delete
                await _context.SaveChangesAsync();


                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to delete client with ID {id}", ex);
            }
        }


        // Note: Service history functionality would require additional tables
        // For now, returning empty lists to maintain interface compatibility
        public Task<List<ServiceRecord>> GetClientServiceHistoryAsync(int clientId)
        {
            return Task.FromResult(new List<ServiceRecord>());
        }


        public Task<ServiceRecord> AddServiceRecordAsync(ServiceRecord serviceRecord)
        {
            return Task.FromResult(serviceRecord);
        }


        public Task<ServiceRecord> UpdateServiceRecordAsync(ServiceRecord serviceRecord)
        {
            return Task.FromResult(serviceRecord);
        }


        private static Client MapCustomerToClient(Customer customer)
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
                },
                CreatedDate = customer.CreatedDate,
                IsActive = customer.IsActive,
                PaymentMethodToken = customer.PaymentMethodToken,
                LastFourDigits = customer.LastFourDigits,
                ServiceHistory = new List<ServiceRecord>() // Empty for now
            };
        }


        private static Customer MapClientToCustomer(Client client)
        {
            return new Customer
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                Street = client.Address.Street,
                City = client.Address.City,
                State = client.Address.State,
                ZipCode = client.Address.ZipCode,
                Country = "USA",
                CreatedDate = client.CreatedDate,
                IsActive = client.IsActive,
                PaymentMethodToken = client.PaymentMethodToken,
                LastFourDigits = client.LastFourDigits
            };
        }
    }
}