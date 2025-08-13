namespace CartifyBLL.ViewModels.Search;

public class SearchResponseDTO
{
    // public List<SearchResultDTO> Results { get; set; } = new List<SearchResultDTO>();
    // public int TotalCount { get; set; }
    // public int PageNumber { get; set; }
    // public int PageSize { get; set; }
    // public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    // public string SearchTerm { get; set; }
    // public string SearchType { get; set; }
    // public bool HasNextPage => PageNumber < TotalPages;
    // public bool HasPreviousPage => PageNumber > 1;
    
    public List<SearchResultDTO> Results { get; set; } = new List<SearchResultDTO>();
}