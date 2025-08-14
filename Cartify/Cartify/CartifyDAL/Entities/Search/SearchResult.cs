namespace CartifyDAL.Entities.Search;

public class SearchResult
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; } // "Product" or "Category"
    public string Description { get; set; }
    public double? Price { get; set; } // Only for products
    public string ImageUrl { get; set; }
    public string Url { get; set; } // URL to navigate to the item
}