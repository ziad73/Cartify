// Document ready function
document.addEventListener('DOMContentLoaded', function () {
    // Animate stats numbers
    animateStatsNumbers();

    // Add hover effects to cards
    setupCardHoverEffects();

    // Setup order status tooltips
    setupStatusTooltips();

    // Setup quick action buttons
    setupQuickActions();

    // Setup responsive sidebar toggle
    setupSidebarToggle();
});

// Animate the stats numbers
function animateStatsNumbers() {
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
}

// Add hover effects to cards
function setupCardHoverEffects() {
    const cards = document.querySelectorAll('.stats-card, .recent-orders, .quick-actions');

    cards.forEach(card => {
        card.addEventListener('mouseenter', function () {
            this.style.transition = 'transform 0.3s ease, box-shadow 0.3s ease';
            this.style.transform = 'translateY(-5px)';
            this.style.boxShadow = '0 5px 15px rgba(0,0,0,0.1)';
        });

        card.addEventListener('mouseleave', function () {
            this.style.transform = '';
            this.style.boxShadow = '0 2px 10px rgba(0,0,0,0.1)';
        });
    });
}

// Add tooltips to order statuses
function setupStatusTooltips() {
    const statusElements = document.querySelectorAll('.order-status');

    statusElements.forEach(status => {
        status.addEventListener('mouseenter', function () {
            const tooltip = document.createElement('div');
            tooltip.className = 'status-tooltip';
            tooltip.textContent = this.textContent + ' - Click to update';

            const rect = this.getBoundingClientRect();
            tooltip.style.position = 'absolute';
            tooltip.style.top = (rect.top - 40) + 'px';
            tooltip.style.left = rect.left + 'px';
            tooltip.style.backgroundColor = '#333';
            tooltip.style.color = '#fff';
            tooltip.style.padding = '5px 10px';
            tooltip.style.borderRadius = '4px';
            tooltip.style.fontSize = '12px';
            tooltip.style.zIndex = '1000';

            document.body.appendChild(tooltip);

            this.addEventListener('mouseleave', function () {
                document.body.removeChild(tooltip);
            });
        });
    });
}

// Add click handlers to quick action buttons
function setupQuickActions() {
    const actionButtons = document.querySelectorAll('.action-btn');

    actionButtons.forEach(button => {
        button.addEventListener('click', function (e) {
            if (this.getAttribute('href') === '#') {
                e.preventDefault();
                // Show loading effect
                const originalText = this.innerHTML;
                this.innerHTML = '<i class="bi bi-arrow-repeat spin"></i> Processing...';

                setTimeout(() => {
                    this.innerHTML = originalText;
                    // In a real app, you would handle the action here
                    console.log('Action clicked:', this.textContent.trim());
                }, 1500);
            }
        });
    });
}

// Sidebar toggle functionality
function setupSidebarToggle() {
    const sidebarToggle = document.getElementById('sidebarToggle');
    const sidebar = document.querySelector('.sidebar');

    if (sidebarToggle && sidebar) {
        sidebarToggle.addEventListener('click', function () {
            sidebar.classList.toggle('collapsed');

            // Store preference in localStorage
            if (sidebar.classList.contains('collapsed')) {
                localStorage.setItem('sidebarCollapsed', 'true');
            } else {
                localStorage.removeItem('sidebarCollapsed');
            }
        });

        // Check for saved preference
        if (localStorage.getItem('sidebarCollapsed') === 'true') {
            sidebar.classList.add('collapsed');
        }
    }
}

// Add spin animation to icons
const style = document.createElement('style');
style.textContent = `
            @keyframes spin {
                0% { transform: rotate(0deg); }
                100% { transform: rotate(360deg); }
            }
            .spin {
                animation: spin 1s linear infinite;
            }
        `;
document.head.appendChild(style);