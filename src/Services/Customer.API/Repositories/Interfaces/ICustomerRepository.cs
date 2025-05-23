﻿using Contracts.Common.Interfaces;
using Customer.API.Persistence;

namespace Customer.API.Repositories.Interfaces
{
    public interface ICustomerRepository : IRepositoryQueryBase<Entities.Customer, int, CustomerContext>
    {
        public Task<Entities.Customer> GetCustomerByUserNameAsync(string username);
    }
}
