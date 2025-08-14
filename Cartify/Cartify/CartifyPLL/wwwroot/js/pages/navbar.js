// Navbar-specific JavaScript
document.addEventListener("DOMContentLoaded", function () {
  // Cart dropdown functionality
  $(".dropdown-toggle").on("click", function (e) {
    e.preventDefault();
    $(this).next(".cart-dropdown").toggle();
  });

  // Close cart dropdown when clicking outside
  $(document).on("click", function (e) {
    if (!$(e.target).closest(".dropdown").length) {
      $(".cart-dropdown").hide();
    }
  });

  // Delete cart items
  $(".delete").on("click", function () {
    $(this).closest(".product-widget").remove();
    updateCartSummary();
  });

  // Update cart summary
  function updateCartSummary() {
    const items = $(".cart-list .product-widget").length;
    $(".cart-summary small").text(items + " Item(s) selected");

    let total = 0;
    $(".cart-list .product-price").each(function () {
      const price = parseFloat(
        $(this).text().replace("$", "").replace(",", "")
      );
      const qty = parseInt($(this).find(".qty").text().replace("x", ""));
      total += price * qty;
    });

    $(".cart-summary h5").text("SUBTOTAL: $" + total.toFixed(2));
  }

  // Search functionality
  $(".header-search form").on("submit", function (e) {
    e.preventDefault();
    const query = $('.header-search input[name="q"]').val();
    const category = $('.header-search select[name="category"]').val();

    if (query.trim()) {
      alert(
        "Searching for: " +
          query +
          " in category: " +
          (category || "All Categories")
      );
      // Redirect to search page with parameters
      // window.location.href = 'search.html?q=' + encodeURIComponent(query) + '&category=' + encodeURIComponent(category);
    }
  });

  // Mobile menu toggle
  $(".menu-toggle a").on("click", function (e) {
    e.preventDefault();
    $("#responsive-nav").toggleClass("active");
  });

  // Navigation active state
  $(".main-nav li a").on("click", function () {
    $(".main-nav li").removeClass("active");
    $(this).parent().addClass("active");
  });
});
