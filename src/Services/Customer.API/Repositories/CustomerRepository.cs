using Customer.API.Repositories.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Customer.API.Persistence;

namespace Customer.API.Repositories
{
    public class CustomerRepository : RepositoryQueryBaseAsync<Entities.Customer, int, CustomerContext>, ICustomerRepository
    {
        public CustomerRepository(CustomerContext dbContext) : base(dbContext)
        {
        }

        public Task<Entities.Customer> GetCustomerByUserNameAsync(string username) =>
            FindByCondition(x => x.UserName.Equals(username))
                .SingleOrDefaultAsync();
    }
}
