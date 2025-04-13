using System.Collections.Generic;
using OneWholesale.Model.Models;

namespace OneWholesale.Repository.Repositories.Repository
{
    public interface IBrandRepository
    {
        Brand GetBrandById(int id);
        IEnumerable<Brand> GetAllBrands();
        bool ManageBrand(string actionType, Brand brand, string user);
        bool DeleteBrand(int id, string user);
      


    }
}
