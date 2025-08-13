using CartifyBLL.ViewModels.Search;

namespace CartifyBLL.Services.SearchService.Abstraction;

public interface ISearchService
{
    // (SearchResponseDTO, string?) SearchAsync(SearchRequestDTO request);
    // (List<string>, string?) GetSearchSuggestionsAsync(string term, int maxResults = 5);
    SearchResponseDTO Search(string query);
}