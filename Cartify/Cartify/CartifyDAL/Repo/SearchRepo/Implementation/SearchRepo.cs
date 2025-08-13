using Cartify.DAL.DataBase;
using CartifyDAL.Entities.product;
using CartifyDAL.Entities.Search;
using CartifyDAL.Repo.SearchRepo.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace CartifyDAL.Repo.SearchRepo.Implementation;

public class SearchRepo : ISearchRepo
{
    private readonly CartifyDbContext _context;

    public SearchRepo(CartifyDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Product> SearchProducts(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return Enumerable.Empty<Product>();

        query = query.Trim().ToLowerInvariant();

        return _context.Product
            .Where(p => p.Name.ToLower().Contains(query)
                        || p.Description.ToLower().Contains(query))
            .ToList();
    }
    }
