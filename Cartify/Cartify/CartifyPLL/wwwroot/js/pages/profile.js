function editProfile() {
  alert("Edit profile functionality would open a form here");
}

function changePassword() {
  alert("Change password functionality would open a form here");
}
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
