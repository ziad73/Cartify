
//         function addToCart(productId) {
//             alert('Product ' + productId + ' added to cart!');
//         }
// // Load navbar
// fetch("navbar.html")
//   .then((response) => response.text())
//   .then((data) => {
//     document.getElementById("navbar-placeholder").innerHTML = data;
//   });
//
// // Load footer
// fetch("footer.html")
//   .then((response) => response.text())
//   .then((data) => {
//     document.getElementById("footer-placeholder").innerHTML = data;
//   });

$(document).ready(function () {
    $('#searchForm').on('submit', function (e) {
        e.preventDefault();
        let query = $('#searchInput').val().trim();
        let category = $('select[name="category"]').val();

        let url = `/Search?query=${encodeURIComponent(query)}`;
        if (category) {
            url += `&category=${encodeURIComponent(category)}`;
        }
        window.location.href = url;
    });
});