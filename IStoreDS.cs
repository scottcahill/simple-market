
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmWeb.Models;

namespace SmWeb.Services
{
    public interface IStoreDS
    {
        Task<IEnumerable<Store>> GetAllStores();
        Task<Store> GetStoreById(string StoreId);
        Task<bool> AddStore(Store Store);
        Task UpdateStore(Store Store);
        Task DeleteStore(string StoreId);
    }
}
