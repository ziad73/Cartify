// Toggle sidebar
document.getElementById('sidebarToggle').addEventListener('click', function () {
    document.querySelector('.sidebar').classList.toggle('active');
});

// Update stats numbers with animation
document.addEventListener('DOMContentLoaded', function () {
    const statsNumbers = document.querySelectorAll('.stats-number');

    statsNumbers.forEach(stat => {
        const finalNumber = stat.textContent;
        const isCurrency = finalNumber.includes('$');
        const numericValue = isCurrency ?
            parseFloat(finalNumber.replace(/[$,]/g, '')) :
            parseInt(finalNumber.replace(/,/g, ''));

        let currentNumber = 0;
        const increment = numericValue / 50;
        const timer = setInterval(() => {
            currentNumber += increment;
            if (currentNumber >= numericValue) {
                currentNumber = numericValue;
                clearInterval(timer);
            }

            if (isCurrency) {
                stat.textContent = '$' + Math.floor(currentNumber).toLocaleString();
            } else {
                stat.textContent = Math.floor(currentNumber).toLocaleString();
            }
        }, 50);
    });

    // Set active nav link
    const currentPath = window.location.pathname;
    const navLinks = document.querySelectorAll('.sidebar .nav-link');

    navLinks.forEach(link => {
        if (link.getAttribute('href') === currentPath) {
            link.classList.add('active');
        } else {
            link.classList.remove('active');
        }
    });
});