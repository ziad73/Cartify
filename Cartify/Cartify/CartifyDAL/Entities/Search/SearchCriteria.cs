namespace CartifyDAL.Entities.Search;

public class SearchCriteria
{
    public string SearchTerm { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SearchType { get; set; } = "All";
}