function saveSettings() {
  // Collect all form values
  const settings = {
    storeName: document.getElementById("storeName").value,
    storeDescription: document.getElementById("storeDescription").value,
    storeEmail: document.getElementById("storeEmail").value,
    storePhone: document.getElementById("storePhone").value,
    storeAddress: document.getElementById("storeAddress").value,
    currency: document.getElementById("currency").value,
    taxRate: document.getElementById("taxRate").value,
    freeShippingThreshold: document.getElementById("freeShippingThreshold")
      .value,
    defaultShippingCost: document.getElementById("defaultShippingCost").value,
    maintenanceMode: document.getElementById("maintenanceMode").checked,
    userRegistration: document.getElementById("userRegistration").checked,
    emailNotifications: document.getElementById("emailNotifications").checked,
    productReviews: document.getElementById("productReviews").checked,
    wishlistFeature: document.getElementById("wishlistFeature").checked,
    compareProducts: document.getElementById("compareProducts").checked,
    stockAlerts: document.getElementById("stockAlerts").checked,
    analyticsTracking: document.getElementById("analyticsTracking").checked,
    twoFactorAuth: document.getElementById("twoFactorAuth").checked,
    sslCertificate: document.getElementById("sslCertificate").checked,
    sessionTimeout: document.getElementById("sessionTimeout").checked,
  };

  console.log("Saving settings:", settings);
  alert("Settings saved successfully!");
}

function resetSettings() {
  if (
    confirm("Are you sure you want to reset all settings to default values?")
  ) {
    // Reset form values to defaults
    document.getElementById("storeName").value = "Cartify Store";
    document.getElementById("storeDescription").value =
      "Your one-stop shop for all products and gadgets.";
    document.getElementById("storeEmail").value = "contact@cartify.com";
    document.getElementById("storePhone").value = "+021-95-51-84";
    document.getElementById("storeAddress").value =
      "1734 Stonecoal Road, New York, NY 10001";
    document.getElementById("currency").value = "USD";
    document.getElementById("taxRate").value = "8.5";
    document.getElementById("freeShippingThreshold").value = "50";
    document.getElementById("defaultShippingCost").value = "5.99";

    // Reset toggles
    document.getElementById("maintenanceMode").checked = false;
    document.getElementById("userRegistration").checked = true;
    document.getElementById("emailNotifications").checked = true;
    document.getElementById("productReviews").checked = true;
    document.getElementById("wishlistFeature").checked = true;
    document.getElementById("compareProducts").checked = true;
    document.getElementById("stockAlerts").checked = true;
    document.getElementById("analyticsTracking").checked = true;
    document.getElementById("twoFactorAuth").checked = false;
    document.getElementById("sslCertificate").checked = true;
    document.getElementById("sessionTimeout").checked = true;

    alert("Settings reset to default values!");
  }
}

// Add click handlers for navigation
document.addEventListener("DOMContentLoaded", function () {
  const navLinks = document.querySelectorAll(".admin-nav .nav-link");

  navLinks.forEach((link) => {
    link.addEventListener("click", function () {
      navLinks.forEach((l) => l.classList.remove("active"));
      this.classList.add("active");
    });
  });
});
