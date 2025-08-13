using CartifyBLL.ViewModels.Search;

namespace CartifyPLL.Models;

public class SearchViewModel
{
    public SearchResponseDTO SearchResponse { get; set; } = new SearchResponseDTO();
    public string SearchTerm { get; set; }
    public string SearchType { get; set; } = "All";
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}