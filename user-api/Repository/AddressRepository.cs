using Microsoft.EntityFrameworkCore;
using user_api.Data;
using user_api.Models;
using user_api.Repository.IRepository;

namespace user_api.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AddressRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(Address address)
        {
            try
            {
                await _dbContext.Addresses.AddAsync(address);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Address> GetAsync(Guid id)
        {
            var adresses = await _dbContext.Addresses.FirstOrDefaultAsync(x=>x.Id == id);
            return adresses!;
        }

        public List<Address> GetAddressAsync(Guid userId)
        {
            var adresses = _dbContext.Addresses.Where(x=>x.UserId == userId).ToList();
            return adresses;
        }

        public async Task UpdateAsync(Address address)
        {
            try
            {
                _dbContext.Addresses.Update(address);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task DeleteAsync(Address address)
        {
            try
            {
                _dbContext.Addresses.Remove(address);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
