document.addEventListener('DOMContentLoaded', function() {
    document.querySelectorAll('form').forEach(form => {
        form.addEventListener('submit', async function(e) {
            const submitButton = form.querySelector('button[type="submit"]');
            if (!submitButton) return;

            // Store original button content
            const originalContent = submitButton.innerHTML;
            
            // Add loading state
            const setLoading = () => {
                submitButton.disabled = true;
                submitButton.innerHTML = `
                    <svg class="animate-spin -ml-1 mr-2 h-4 w-4 inline" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                        <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                        <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                    </svg>
                    <span>Processing...</span>
                `;
            };

            // Reset button to original state
            const resetButton = () => {
                submitButton.disabled = false;
                submitButton.innerHTML = originalContent;
            };

            try {
                // Set initial loading state
                setLoading();

                // Handle client-side validation failure
                if (!form.checkValidity()) {
                    resetButton();
                    return;
                }

                // Handle form submission
                const formData = new FormData(form);
                const response = await fetch(form.action, {
                    method: form.method,
                    body: formData
                });

                // Reset button on error responses
                if (!response.ok) {
                    resetButton();
                    return;
                }

                // On success, either the page will redirect or we'll process the response
                const contentType = response.headers.get('content-type');
                if (contentType && contentType.includes('application/json')) {
                    const result = await response.json();
                    // Handle JSON response if needed
                    resetButton();
                }
                // Otherwise, let the form submission handle the response (like redirects)
            } catch (error) {
                console.error('Form submission error:', error);
                resetButton();
            }
        });

        // Handle client-side validation errors
        form.addEventListener('invalid', () => {
            const submitButton = form.querySelector('button[type="submit"]');
            if (submitButton) {
                submitButton.disabled = false;
                submitButton.innerHTML = submitButton.getAttribute('data-original-content') || 'Save Changes';
            }
        }, true);
    });
});
