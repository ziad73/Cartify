using CartifyBLL.Services.SearchService.Abstraction;
using CartifyBLL.ViewModels.Search;
using CartifyPLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace CartifyPLL.Controllers;

public class SearchController : Controller
{

  private readonly ISearchService _searchService;

  public SearchController(ISearchService searchService)
  {
      _searchService = searchService;
  }

  [HttpGet]
  public IActionResult Index(string query)
  {
      if (string.IsNullOrWhiteSpace(query))
      {
          return View(new SearchResponseDTO());
      }

      var results = _searchService.Search(query);
      return View(results);
  }
}