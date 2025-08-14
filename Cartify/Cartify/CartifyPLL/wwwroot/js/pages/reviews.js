document.addEventListener("DOMContentLoaded", function () {

    function loadRatingStats(productId) {
        const reviewForm = document.getElementById("reviewForm");
        if (!reviewForm) return; 

        const ratingUrl = reviewForm.dataset.ratingUrl;
        if (!ratingUrl) {
            console.error("Rating URL is not defined in data-rating-url attribute.");
            return;
        }

        fetch(`${ratingUrl}?productId=${productId}`)
            .then(res => {
                if (!res.ok) {
                    throw new Error(`Network response was not ok: ${res.statusText}`);
                }
                return res.json();
            })
            .then(data => {
                
                document.getElementById("avg-rating").textContent = data.average.toFixed(1);

                
                const avgStars = document.getElementById("avg-stars");
                avgStars.innerHTML = ""; 
                for (let i = 1; i <= 5; i++) {
                    avgStars.innerHTML += `<i class="fa fa-star${i <= Math.round(data.average) ? '' : '-o'}"></i>`;
                }

                
                const breakdown = document.getElementById("rating-breakdown");
                breakdown.innerHTML = ""; 
                const totalReviews = data.counts.reduce((a, b) => a + b, 0);

                for (let i = 5; i >= 1; i--) {
                    const count = data.counts[i - 1];
                    const percentage = totalReviews > 0 ? (count / totalReviews) * 100 : 0;

                    const starsHtml = '<i class="fa fa-star"></i>'.repeat(i) + '<i class="fa fa-star-o"></i>'.repeat(5 - i);

                    breakdown.innerHTML += `
                        <li>
                            <div class="rating-stars">${starsHtml}</div>
                            <div class="rating-progress">
                                <div style="width: ${percentage}%"></div>
                            </div>
                            <span class="sum">${count}</span>
                        </li>
                    `;
                }
            })
            .catch(err => console.error("Error loading rating stats:", err));
    }

   
    function addNewReviewToList(review) {
        const reviewsList = document.getElementById("reviewsList");
        if (!reviewsList) return;

        
        const noReviewsMessage = reviewsList.querySelector("li > p");
        if (noReviewsMessage && noReviewsMessage.textContent.includes("no reviews yet")) {
            noReviewsMessage.parentElement.remove();
        }

        
        const newReviewItem = document.createElement("li");

        
        let starsHtml = '';
        for (let i = 1; i <= 5; i++) {
            starsHtml += `<i class="fa fa-star${i <= review.rating ? '' : '-o empty'}"></i>`;
        }

        newReviewItem.innerHTML = `
            <div class="review-heading">
                <h5 class="name">${review.userName}</h5>
                <p class="date">${new Date(review.createdAt).toLocaleString()}</p>
                <div class="review-rating">${starsHtml}</div>
            </div>
            <div class="review-body">
                <p>${review.comment}</p>
            </div>
        `;

        
        reviewsList.prepend(newReviewItem);
    }

    
    const reviewForm = document.getElementById("reviewForm");
    if (reviewForm) {
        
        const initialProductId = reviewForm.dataset.productId;
        if (initialProductId) {
            loadRatingStats(initialProductId);
        }

        
        reviewForm.addEventListener("submit", function (event) {
            event.preventDefault(); 

            const submitButton = this.querySelector("button[type='submit']");
            submitButton.disabled = true;
            submitButton.textContent = "Submitting...";

            const formData = new FormData(this);
            const addUrl = this.dataset.addUrl;

            fetch(addUrl, {
                method: "POST",
                body: formData,
                headers: {
                    'RequestVerificationToken': formData.get('__RequestVerificationToken')
                }
            })
                .then(response => {
                    if (!response.ok) {
                        return response.json().then(err => { throw new Error(err.message || 'Failed to submit review.') });
                    }
                    return response.json();
                })
                .then(data => {
                    if (data.success) {
                        
                        addNewReviewToList(data.review);
                        
                        loadRatingStats(data.review.productId);
                        
                        this.reset();
                    } else {
                        alert(data.message || "An unknown error occurred.");
                    }
                })
                .catch(error => {
                    console.error("Error submitting review:", error);
                    alert(error.message || "An error occurred. Please try again.");
                })
                .finally(() => {
                    
                    submitButton.disabled = false;
                    submitButton.textContent = "Submit";
                });
        });
    }
});
