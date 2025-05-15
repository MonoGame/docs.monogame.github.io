document.addEventListener('DOMContentLoaded', function () {
    // Find all question-answer containers
    const containers = document.querySelectorAll('.question-answer');

    containers.forEach(function (container) {
        // Make the container focusable
        container.setAttribute('tabindex', '0');

        // Add click event to reveal content
        container.addEventListener('click', function () {
            this.classList.add('revealed');
        });

        // Add keyboard accessibility
        container.addEventListener('keydown', function (e) {
            if (e.key === 'Enter' || e.key === ' ') {
                e.preventDefault();
                this.classList.add('revealed');
            }
        });

        // Optional: Allow re-hiding by right-click
        container.addEventListener('contextmenu', function (e) {
            e.preventDefault();
            this.classList.remove('revealed');
        });
    });
});