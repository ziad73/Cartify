// Initialize admin dashboard functionality when DOM is loaded
document.addEventListener('DOMContentLoaded', function() {
    // Update stats numbers with animation
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

    // Add click handlers for navigation
    const navLinks = document.querySelectorAll('.admin-nav .nav-link');
    
    navLinks.forEach(link => {
        link.addEventListener('click', function() {
            navLinks.forEach(l => l.classList.remove('active'));
            this.classList.add('active');
        });
    });
}); 