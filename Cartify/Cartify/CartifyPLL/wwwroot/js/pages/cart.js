// Cart functionality
function updateCart() {
  // Update cart totals based on quantity changes
  let total = 0;
  document.querySelectorAll(".cart-list tbody tr").forEach((row) => {
    const price = parseFloat(
      row.querySelector(".price").textContent.replace("$", "")
    );
    const qty = parseInt(row.querySelector(".input-number input").value);
    const rowTotal = price * qty;
    row.querySelector(".total").textContent = "$" + rowTotal.toFixed(2);
    total += rowTotal;
  });

  // Update summary
  const subtotal = total;
  const shipping = 50;
  const tax = subtotal * 0.1;
  const grandTotal = subtotal + shipping + tax;

  document.querySelector(".summary-item:nth-child(1) .price").textContent =
    "$" + subtotal.toFixed(2);
  document.querySelector(".summary-item:nth-child(3) .price").textContent =
    "$" + tax.toFixed(2);
  document.querySelector(".summary-item:nth-child(4) .price").textContent =
    "$" + grandTotal.toFixed(2);
}

// Initialize cart functionality when DOM is loaded
document.addEventListener("DOMContentLoaded", function () {
  // Remove item from cart
  document.querySelectorAll(".delete").forEach((button) => {
    button.addEventListener("click", function () {
      this.closest("tr").remove();
      updateCart();
    });
  });

  // Quantity change handlers
  document.querySelectorAll(".qty-up").forEach((button) => {
    button.addEventListener("click", function () {
      const input = this.parentNode.querySelector("input");
      input.value = Math.min(parseInt(input.value) + 1, 10);
      updateCart();
    });
  });

  document.querySelectorAll(".qty-down").forEach((button) => {
    button.addEventListener("click", function () {
      const input = this.parentNode.querySelector("input");
      input.value = Math.max(parseInt(input.value) - 1, 1);
      updateCart();
    });
  });
});

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
