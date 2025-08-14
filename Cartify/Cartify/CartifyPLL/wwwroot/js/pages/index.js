fetch("navbar.html")
  .then((response) => response.text())
  .then(
    (data) => (document.getElementById("navbar-placeholder").innerHTML = data)
  );
fetch("footer.html")
  .then((response) => response.text())
  .then(
    (data) => (document.getElementById("footer-placeholder").innerHTML = data)
  );

// Product interaction functions
function addToWishlist(productId) {
  alert("Product " + productId + " added to wishlist!");
}

function addToCompare(productId) {
  alert("Product " + productId + " added to compare!");
}

function quickView(productId) {
  window.location.href = "product-details.html?id=" + productId;
}

function addToCart(productId) {
  alert("Product " + productId + " added to cart!");
}

// Add event listeners to product buttons
document.addEventListener("DOMContentLoaded", function () {
  // Wishlist buttons
  document.querySelectorAll(".add-to-wishlist").forEach(function (btn, index) {
    btn.addEventListener("click", function () {
      addToWishlist(index + 1);
    });
  });

  // Compare buttons
  document.querySelectorAll(".add-to-compare").forEach(function (btn, index) {
    btn.addEventListener("click", function () {
      addToCompare(index + 1);
    });
  });

  // Quick view buttons
  document.querySelectorAll(".quick-view").forEach(function (btn, index) {
    btn.addEventListener("click", function () {
      quickView(index + 1);
    });
  });

  // Add to cart buttons
  document.querySelectorAll(".add-to-cart-btn").forEach(function (btn, index) {
    btn.addEventListener("click", function () {
      addToCart(index + 1);
    });
  });

  // Product name links
  document.querySelectorAll(".product-name a").forEach(function (link, index) {
    link.addEventListener("click", function (e) {
      e.preventDefault();
      quickView(index + 1);
    });
  });
});
