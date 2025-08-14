function submitContact(event) {
  event.preventDefault();
  alert("Thank you for your message! We will get back to you soon.");
  document.getElementById("contactForm").reset();
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
