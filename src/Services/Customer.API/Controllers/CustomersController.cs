using Customer.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Customer.API.Controllers
{
    public static class CustomersController
    {
        public static void MapCustomersAPI(this WebApplication app)
        {
            app.MapGet("/api/customers/{username}",
                async (string username, ICustomerService customerService) =>
                {
                    var result = await customerService.GetCustomerByUsernameAsync(username);
                    return result != null ? result : Results.NotFound();
                });
        }
    }
}