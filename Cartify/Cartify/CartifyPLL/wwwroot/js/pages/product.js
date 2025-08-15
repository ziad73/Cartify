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
function changeImage(thumbnail, imageSrc) {
    document.getElementById("main-product-img").src = imageSrc;
}
