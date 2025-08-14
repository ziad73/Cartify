function validateLoginForm() {
    const email = document.getElementById('email');
    const password = document.getElementById('password');
    const emailError = document.getElementById('emailError');
    const passwordError = document.getElementById('passwordError');

    let isValid = true;
    emailError.textContent = '';
    passwordError.textContent = '';

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email.value)) {
        emailError.textContent = 'Invalid email format';
        isValid = false;
    }

    if (password.value.length < 6) {
        passwordError.textContent = 'Minimum 6 characters';
        isValid = false;
    }

    return isValid;
}
