namespace CartifyBLL.ViewModels.Search;

public class SearchResultDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string ImageUrl { get; set; }
    public string Url { get; set; }
    public string Category { get; set; }
    public int CategoryId { get; set; }
    public int StockQuantity { get; set; }
}