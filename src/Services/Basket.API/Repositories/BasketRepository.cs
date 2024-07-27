using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Contracts.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCacheService;
        private readonly ISerializeService _serializeService;
        private readonly ILogger _logger;
        public BasketRepository(IDistributedCache redisCacheService, ISerializeService serializeService, ILogger logger)
        {
            _redisCacheService = redisCacheService;
            _serializeService = serializeService;
            _logger = logger;
        }


        public async Task<Cart?> GetBasketByUserName(string username)
        {
            _logger.LogInformation($"BEGIN: GetBasketByUserName {username}");
            var basket = await _redisCacheService.GetStringAsync(username);
            _logger.LogInformation($"END: GetBasketByUserName {username}");

            return string.IsNullOrEmpty(basket) ? null : _serializeService.Deserialize<Cart>(basket);
        }

        public async Task<Cart> UpdateBasket(Cart cart, DistributedCacheEntryOptions options = null)
        {
            _logger.LogInformation($"BEGIN: UpdateBasket for {cart.UserName}");

            if (options != null)
                await _redisCacheService.SetStringAsync(cart.UserName,
                    _serializeService.Serialize(cart), options);
            else
                await _redisCacheService.SetStringAsync(cart.UserName,
                    _serializeService.Serialize(cart));

            _logger.LogInformation($"END: UpdateBasket for {cart.UserName}");

            return await GetBasketByUserName(cart.UserName);
        }

        public async Task<bool> DeleteBasketFromUserName(string username)
        {
            try
            {
                _logger.LogInformation($"BEGIN: DeleteBasketFromUserName {username}");
                await _redisCacheService.RemoveAsync(username);
                _logger.LogInformation($"END: DeleteBasketFromUserName {username}");

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("Error DeleteBasketFromUserName: " + e.Message);
                throw;
            }
        }
    }
}
