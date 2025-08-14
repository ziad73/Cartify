function validateRegisterForm() {
  let isValid = true;
  const firstName = document.getElementById("firstName").value;
  const lastName = document.getElementById("lastName").value;
  const email = document.getElementById("email").value;
  const phone = document.getElementById("phone").value;
  const password = document.getElementById("password").value;
  const confirmPassword = document.getElementById("confirmPassword").value;
  const terms = document.getElementById("terms").checked;

  // Reset error messages
  document
    .querySelectorAll(".error-message")
    .forEach((el) => (el.textContent = ""));

  // First name validation
  if (firstName.length < 2) {
    document.getElementById("firstNameError").textContent =
      "First name must be at least 2 characters long";
    isValid = false;
  }

  // Last name validation
  if (lastName.length < 2) {
    document.getElementById("lastNameError").textContent =
      "Last name must be at least 2 characters long";
    isValid = false;
  }

  // Email validation
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  if (!emailRegex.test(email)) {
    document.getElementById("emailError").textContent =
      "Please enter a valid email address";
    isValid = false;
  }

  // Phone validation (optional)
  if (
    phone &&
    !/^[\+]?[1-9][\d]{0,15}$/.test(phone.replace(/[\s\-\(\)]/g, ""))
  ) {
    document.getElementById("phoneError").textContent =
      "Please enter a valid phone number";
    isValid = false;
  }

  // Password validation
  if (password.length < 8) {
    document.getElementById("passwordError").textContent =
      "Password must be at least 8 characters long";
    isValid = false;
  } else if (!/(?=.*[a-z])(?=.*[A-Z])(?=.*\d)/.test(password)) {
    document.getElementById("passwordError").textContent =
      "Password must contain at least one uppercase letter, one lowercase letter, and one number";
    isValid = false;
  }

  // Confirm password validation
  if (password !== confirmPassword) {
    document.getElementById("confirmPasswordError").textContent =
      "Passwords do not match";
    isValid = false;
  }

  // Terms validation
  if (!terms) {
    alert("You must agree to the Terms & Conditions and Privacy Policy");
    isValid = false;
  }

  if (isValid) {
    // Simulate registration - in real app, this would send data to server
    alert(
      "Registration successful! Please check your email to verify your account."
    );
      window.location.href = "/Account/Login";
  }

  return false; // Prevent form submission for demo
}
