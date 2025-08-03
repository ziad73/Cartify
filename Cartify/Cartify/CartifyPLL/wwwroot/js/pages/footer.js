// Footer links click tracking
$(".footer-links a").on("click", function (e) {
  const linkText = $(this).text();
  const linkHref = $(this).attr("href");

  // Track footer link clicks (for analytics)
  console.log("Footer link clicked:", linkText, linkHref);

  // If it's a demo link, prevent navigation
  if (linkHref === "#") {
    e.preventDefault();
    alert("This is a demo link: " + linkText);
  }
});

// Social media links
$(".newsletter-follow a").on("click", function (e) {
  e.preventDefault();
  const platform = $(this).find("i").attr("class").split("-")[2];
  alert("Opening " + platform + " in a new window...");
  // In a real implementation, this would open the actual social media profile
  // window.open('https://' + platform + '.com/yourprofile', '_blank');
});

// Payment method links
$(".footer-payments a").on("click", function (e) {
  e.preventDefault();
  const paymentMethod = $(this).find("i").attr("class").split("-")[2];
  alert("Payment method: " + paymentMethod);
});

// Smooth scroll to footer sections
$('a[href^="#"]').on("click", function (e) {
  e.preventDefault();
  const target = $(this.getAttribute("href"));
  if (target.length) {
    $("html, body").animate(
      {
        scrollTop: target.offset().top,
      },
      1000
    );
  }
});

// Update copyright year
const currentYear = new Date().getFullYear();
$(".copyright script").replaceWith(currentYear);
