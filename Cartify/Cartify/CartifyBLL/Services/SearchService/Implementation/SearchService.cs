using AutoMapper;
using CartifyBLL.Services.SearchService.Abstraction;
using CartifyBLL.ViewModels.Search;
using CartifyDAL.Entities.Search;
using CartifyDAL.Repo.SearchRepo.Abstraction;

namespace CartifyBLL.Services.SearchService.Implementation;

public class SearchService : ISearchService
{
    // private readonly ISearchRepo _searchRepo;
    // private readonly IMapper _mapper;
    //
    // public SearchService(ISearchRepo searchRepo, IMapper mapper)
    // {
    //     _searchRepo = searchRepo;
    //     _mapper = mapper;
    // }
    //
    // public (SearchResponseDTO, string?) SearchAsync(SearchRequestDTO request)
    // {
    //     try
    //     {
    //         // Map DTO to domain model
    //         var criteria = _mapper.Map<SearchCriteria>(request);
    //
    //         // Get search results and total count
    //         var (results, resultsError) = _searchRepo.SearchAsync(criteria);
    //         if (!string.IsNullOrEmpty(resultsError))
    //             return (new SearchResponseDTO(), resultsError);
    //
    //         var (totalCount, countError) = _searchRepo.GetSearchResultsCountAsync(criteria);
    //         if (!string.IsNullOrEmpty(countError))
    //             return (new SearchResponseDTO(), countError);
    //
    //         // Map results to DTOs
    //         var resultDtos = _mapper.Map<List<SearchResultDTO>>(results);
    //
    //         var response = new SearchResponseDTO
    //         {
    //             Results = resultDtos,
    //             TotalCount = totalCount,
    //             PageNumber = request.PageNumber,
    //             PageSize = request.PageSize,
    //             SearchTerm = request.SearchTerm,
    //             SearchType = request.SearchType
    //         };
    //
    //         return (response, null);
    //     }
    //     catch (Exception ex)
    //     {
    //         return (new SearchResponseDTO(), ex.Message);
    //     }
    // }
    //
    // public (List<string>, string?) GetSearchSuggestionsAsync(string term, int maxResults = 5)
    // {
    //     return _searchRepo.GetSearchSuggestionsAsync(term, maxResults);
    // }
    
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
            Id = p.ProductId, // Ensure property name matches your entity
            Name = p.Name,
            Price = p.Price,
            ImageUrl = p.ImageUrl
        }).ToList();

        return new SearchResponseDTO { Results = results };
    }
    }
