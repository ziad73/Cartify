using CartifyBLL.Services.ProductReviewServices.Abstraction;
using CartifyBLL.ViewModels.Products.ProductReview;
using CartifyDAL.Entities.product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CartifyPLL.Controllers
{
    [AllowAnonymous]
    public class ProductReviewController : Controller
    {
        private readonly IProductReviewServices reviewService;

        public ProductReviewController(IProductReviewServices reviewService)
        {
            this.reviewService = reviewService;
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(CreateProductReview model)
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = User.FindFirst("FullName")?.Value ?? User.Identity?.Name;

            if (string.IsNullOrEmpty(userIdStr))
                return Unauthorized();

            model.UserId = userIdStr;
            model.ReviewerName = userName;

            ModelState.Remove(nameof(model.UserId));
            ModelState.Remove(nameof(model.ReviewerName));

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(kvp => kvp.Value.Errors.Count > 0)
                    .Select(kvp => new
                    {
                        field = kvp.Key,
                        errors = kvp.Value.Errors.Select(e => e.ErrorMessage)
                    });

                return BadRequest("Invalid review data: " +
                    System.Text.Json.JsonSerializer.Serialize(errors));
            }

            var (ok, err) = await reviewService.AddReviewAsync(model);
            if (!ok)
                return BadRequest(err ?? "Failed to add review.");

            return Json(new
            {
                success = true,
                review = new
                {
                    productId = model.ProductId,
                    userName = model.ReviewerName,
                    rating = model.Rating,
                    comment = model.Comment,
                    createdAt = DateTime.Now
                }
            });
        }



        public async Task<IActionResult> GetReviews(int ProductId)
        {
            var result = await reviewService.GetReviewByProductIdAsync(ProductId);
            if (result.Item2 != null)
                return BadRequest(result.Item2);
            return Json(result.Item1);
        }
        [HttpGet]
        public async Task<IActionResult> GetRatingStats(int productId)
        {
            var reviews = await reviewService.GetReviewByProductIdAsync(productId);
            if (reviews.Item2 != null)
                return BadRequest(reviews.Item2);

            var ratings = reviews.Item1;
            if (ratings.Count == 0)
            {
                return Ok(new
                {
                    average = 0,
                    counts = new int[] { 0, 0, 0, 0, 0 }
                });
            }

            var average = ratings.Average(r => r.Rating);
            var counts = new int[5];
            for (int i = 1; i <= 5; i++)
            {
                counts[i - 1] = ratings.Count(r => r.Rating == i);
            }

            return Ok(new
            {
                average,
                counts
            });
        }

    }
}
