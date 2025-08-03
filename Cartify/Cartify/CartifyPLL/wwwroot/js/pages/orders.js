function filterOrders() {
  const statusFilter = document.querySelector("select").value;
  const rows = document.querySelectorAll(".order-row");

  rows.forEach((row) => {
    const status = row.getAttribute("data-status");
    if (!statusFilter || status === statusFilter) {
      row.style.display = "";
    } else {
      row.style.display = "none";
    }
  });
}

function searchOrders() {
  const searchTerm = document
    .querySelector('input[type="text"]')
    .value.toLowerCase();
  const rows = document.querySelectorAll(".order-row");

  rows.forEach((row) => {
    const orderId = row
      .querySelector("td:first-child")
      .textContent.toLowerCase();
    if (orderId.includes(searchTerm)) {
      row.style.display = "";
    } else {
      row.style.display = "none";
    }
  });
}

function filterByDate() {
  const dateFilter = document.querySelector('input[type="date"]').value;
  const rows = document.querySelectorAll(".order-row");

  rows.forEach((row) => {
    const orderDate = row.getAttribute("data-date");
    if (!dateFilter || orderDate === dateFilter) {
      row.style.display = "";
    } else {
      row.style.display = "none";
    }
  });
}

function resetFilters() {
  document.querySelector("select").value = "";
  document.querySelector('input[type="text"]').value = "";
  document.querySelector('input[type="date"]').value = "";

  const rows = document.querySelectorAll(".order-row");
  rows.forEach((row) => {
    row.style.display = "";
  });
}

function viewOrderDetails(orderId) {
  alert(`Viewing details for Order #${orderId}`);
  // In a real application, this would navigate to an order details page
}

function trackOrder(orderId) {
  alert(`Tracking Order #${orderId}`);
  // In a real application, this would show tracking information
}

function cancelOrder(orderId) {
  if (confirm(`Are you sure you want to cancel Order #${orderId}?`)) {
    alert(`Order #${orderId} has been cancelled`);
    // In a real application, this would send a cancellation request to the server
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
