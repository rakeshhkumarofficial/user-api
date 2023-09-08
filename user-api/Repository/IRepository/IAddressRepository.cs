using user_api.Models;

namespace user_api.Repository.IRepository
{
    public interface IAddressRepository
    {
        public Task CreateAsync(Address address);
        public Task UpdateAsync(Address address);
        public List<Address> GetAddressAsync(Guid userId);
        public Task<Address> GetAsync(Guid id);
        public Task DeleteAsync(Address address);
    }
}
