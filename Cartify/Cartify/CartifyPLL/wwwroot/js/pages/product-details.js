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

// Change main product image
function changeImage(thumbnail, imageSrc) {
  document.getElementById("main-product-image").src = imageSrc;

  // Remove active class from all thumbnails
  document.querySelectorAll(".thumbnail").forEach((thumb) => {
    thumb.classList.remove("active");
  });

  // Add active class to clicked thumbnail
  thumbnail.classList.add("active");
}

// Quantity controls
document.addEventListener("DOMContentLoaded", function () {
  const qtyUp = document.querySelector(".qty-up");
  const qtyDown = document.querySelector(".qty-down");
  const quantityInput = document.getElementById("quantity");

  qtyUp.addEventListener("click", function () {
    quantityInput.value = parseInt(quantityInput.value) + 1;
  });

  qtyDown.addEventListener("click", function () {
    if (parseInt(quantityInput.value) > 1) {
      quantityInput.value = parseInt(quantityInput.value) - 1;
    }
  });
});

// Add to cart functionality
function addToCart() {
  const quantity = document.getElementById("quantity").value;
  alert(`Added ${quantity} item(s) to cart!`);
  // Here you would typically send data to backend
}

// Buy now functionality
function buyNow() {
  const quantity = document.getElementById("quantity").value;
  alert(`Proceeding to checkout with ${quantity} item(s)!`);
  window.location.href = "checkout.html";
}

// Star rating functionality
document.addEventListener("DOMContentLoaded", function () {
  const stars = document.querySelectorAll(".star");
  let selectedRating = 0;

  stars.forEach((star) => {
    star.addEventListener("click", function () {
      const rating = this.getAttribute("data-rating");
      selectedRating = rating;

      // Update star display
      stars.forEach((s, index) => {
        if (index < rating) {
          s.classList.add("active");
        } else {
          s.classList.remove("active");
        }
      });
    });

    star.addEventListener("mouseenter", function () {
      const rating = this.getAttribute("data-rating");
      stars.forEach((s, index) => {
        if (index < rating) {
          s.classList.add("active");
        } else {
          s.classList.remove("active");
        }
      });
    });

    star.addEventListener("mouseleave", function () {
      stars.forEach((s, index) => {
        if (index < selectedRating) {
          s.classList.add("active");
        } else {
          s.classList.remove("active");
        }
      });
    });
  });

  // Review form submission
  document
    .getElementById("reviewForm")
    .addEventListener("submit", function (e) {
      e.preventDefault();
      if (selectedRating === 0) {
        alert("Please select a rating");
        return;
      }
      alert("Review submitted successfully!");
      this.reset();
      selectedRating = 0;
      stars.forEach((star) => star.classList.remove("active"));
    });
});
