// auth.js - Shared authentication scripts
document.addEventListener('DOMContentLoaded', function () {
    // Initialize all password toggles
    document.querySelectorAll('input[type="password"]').forEach(function (passwordInput) {
        if (!passwordInput.parentNode.querySelector('.bi-eye')) {
            const passwordToggle = document.createElement('i');
            passwordToggle.className = 'bi bi-eye position-absolute top-50 end-0 translate-middle-y pe-3 text-secondary';
            passwordToggle.style.cursor = 'pointer';
            passwordInput.parentNode.appendChild(passwordToggle);

            passwordToggle.addEventListener('click', function () {
                if (passwordInput.type === 'password') {
                    passwordInput.type = 'text';
                    passwordToggle.classList.replace('bi-eye', 'bi-eye-slash');
                } else {
                    passwordInput.type = 'password';
                    passwordToggle.classList.replace('bi-eye-slash', 'bi-eye');
                }
            });
        }
    });

    // Handle form submissions with loading indicators
    document.querySelectorAll('.auth-form').forEach(function (form) {
        form.addEventListener('submit', function (event) {
            const submitBtn = form.querySelector('button[type="submit"]');
            if (submitBtn) {
                const spinner = submitBtn.querySelector('.spinner-border');
                if (spinner) {
                    submitBtn.disabled = true;
                    spinner.classList.remove('d-none');
                }
            }
        });
    });
});

// Add these to your existing auth.js

// Password strength indicator
function initPasswordStrength() {
    const passwordInputs = document.querySelectorAll('input[type="password"][id="Password"]');

    passwordInputs.forEach(input => {
        const strengthBar = document.createElement('div');
        strengthBar.className = 'password-strength';
        strengthBar.innerHTML = '<div class="password-strength-bar"></div>';
        input.parentNode.insertBefore(strengthBar, input.nextSibling);

        input.addEventListener('input', function () {
            const strength = calculatePasswordStrength(this.value);
            const bar = strengthBar.querySelector('.password-strength-bar');

            bar.style.width = strength.percentage + '%';
            bar.style.background = strength.color;
        });
    });

    function calculatePasswordStrength(password) {
        let strength = 0;

        // Length (max 30 points)
        strength += Math.min(password.length * 3, 30);

        // Contains numbers
        if (/\d/.test(password)) strength += 10;

        // Contains special chars
        if (/[!@#$%^&*(),.?":{}|<>]/.test(password)) strength += 20;

        // Contains both lower and upper case
        if (/[a-z]/.test(password) && /[A-Z]/.test(password)) strength += 20;

        const percentage = Math.min(Math.round(strength / 80 * 100), 100);

        return {
            percentage: percentage,
            color: percentage < 40 ? '#dc3545' :
                percentage < 70 ? '#fd7e14' : '#28a745'
        };
    }
}

// Initialize when DOM loads
document.addEventListener('DOMContentLoaded', function () {
    initPasswordStrength();

    // Terms validation
    const termsCheck = document.getElementById('termsCheck');
    if (termsCheck) {
        termsCheck.addEventListener('change', function () {
            this.setCustomValidity(this.checked ? '' : 'You must agree to the terms');
        });
    }
});

            /* Verification code inputs */
.verification - code {
    border: 1px solid #ced4da;
    border - radius: 8px;
    transition: all 0.2s ease;
    caret - color: transparent;
}

.verification - code:focus {
    border - color: var(--primary - color);
    box - shadow: 0 0 0 0.25rem rgba(67, 97, 238, 0.25);
    outline: none;
}

/* Resend link */
#resendLink.disabled {
    pointer - events: none;
    color: var(--text - muted);
    text - decoration: none;
}

/* Countdown timer */
#countdown {
    display: inline - block;
    min - width: 40px;
}