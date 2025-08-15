function addToCart(productId) {
  alert(`Product ${productId} added to cart!`);
}

function viewProduct(productId) {
    window.location.href = `/Store/Details/${productId}`;
}

function removeFromWishlist(productId) {
  if (
    confirm("Are you sure you want to remove this item from your wishlist?")
  ) {
    const item = document
      .querySelector(`[onclick="removeFromWishlist(${productId})"]`)
      .closest(".wishlist-item");
    item.remove();

    // Update wishlist count
    const remainingItems = document.querySelectorAll(".wishlist-item").length;
    if (remainingItems === 0) {
      document.getElementById("wishlistItems").style.display = "none";
      document.getElementById("emptyWishlist").style.display = "block";
    }

    alert(`Product ${productId} removed from wishlist!`);
  }
}

function addAllToCart() {
  const items = document.querySelectorAll(".wishlist-item");
  if (items.length > 0) {
    alert(`Added ${items.length} items to cart!`);
  } else {
    alert("No items in wishlist to add!");
  }
}

function clearWishlist() {
  if (confirm("Are you sure you want to clear your entire wishlist?")) {
    document.getElementById("wishlistItems").style.display = "none";
    document.getElementById("emptyWishlist").style.display = "block";
    alert("Wishlist cleared!");
  }
}
// Load navbar
fetch("navbar.html")
  .then((response) => response.text())
  .then((data) => {
    document.getElementById("navbar-placeholder").innerHTML = data;
  });

// Load footer
fetch("footer.html")
  .then((response) => response.text())
  .then((data) => {
    document.getElementById("footer-placeholder").innerHTML = data;
  });
