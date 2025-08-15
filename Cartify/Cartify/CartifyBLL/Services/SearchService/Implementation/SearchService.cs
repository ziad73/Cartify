using AutoMapper;
using CartifyBLL.Services.SearchService.Abstraction;
using CartifyBLL.ViewModels.Search;
using CartifyDAL.Entities.Search;
using CartifyDAL.Repo.SearchRepo.Abstraction;

namespace CartifyBLL.Services.SearchService.Implementation;

public class SearchService : ISearchService
{
    

    private readonly ISearchRepo _searchRepo;

    public SearchService(ISearchRepo searchRepo)
    {
        _searchRepo = searchRepo;
    }

    public SearchResponseDTO Search(string query)
    {
        var products = _searchRepo.SearchProducts(query);

        var results = products.Select(p => new SearchResultDTO
        {
            Id = p.ProductId,
            Name = p.Name,
            Price = p.Price,
            ImageUrl = p.ImageUrl,
            StockQuantity = p.StockQuantity,
            Category = p.Category?.Name,
            CategoryId = p.CategoryId,
            Description = p.Description
        }).ToList();

        return new SearchResponseDTO { Results = results };
    }
}
