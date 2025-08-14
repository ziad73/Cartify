$(document).ready(function () {

    // Place Order button click
    $("#place-order").on("click", function (e) {
        e.preventDefault();

        var form = $("#checkoutForm");
        var formData = form.serialize();

        $.ajax({
            url: form.attr("action"),
            type: "POST",
            data: formData,
            headers: {
                "X-Requested-With": "XMLHttpRequest"
            },
            success: function (data) {
                if (data.success) {
                    // ✅ Redirect to Confirmation page with fromAjax=true
                    window.location.href = "/Checkout/Confirmation/" + data.orderId + "?fromAjax=true";
                } else {
                    // Show error message
                    alert(data.message || "An error occurred while placing your order.");
                }
            },
            error: function () {
                alert("An unexpected error occurred. Please try again.");
            }
        });
    });

});
