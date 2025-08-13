using CartifyDAL.Entities.product;
using CartifyDAL.Entities.Search;

namespace CartifyDAL.Repo.SearchRepo.Abstraction;

public interface ISearchRepo
{
    IEnumerable<Product> SearchProducts(string query);
}