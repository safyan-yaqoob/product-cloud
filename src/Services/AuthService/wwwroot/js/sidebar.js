document.addEventListener('DOMContentLoaded', function() {
    const sidebar = document.getElementById('sidebar');
    const toggleButton = document.getElementById('toggleSidebar');
    const sidebarOverlay = document.getElementById('sidebarOverlay');
    const mobileMenuButton = document.getElementById('mobileMenuButton');
    const mobileMenu = document.getElementById('mobileMenu');
    
    // Function to get cookie value
    function getCookie(name) {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);
        if (parts.length === 2) return parts.pop().split(';').shift();
        return null;
    }

    // Function to set cookie
    function setCookie(name, value, days = 365) {
        const date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        const expires = `expires=${date.toUTCString()}`;
        document.cookie = `${name}=${value};${expires};path=/`;
    }
    
    // Check for saved sidebar state
    const isCollapsed = getCookie('sidebar-collapsed') === 'true';
    
    // Initialize sidebar state
    function initSidebar() {
        if (isCollapsed) {
            document.body.classList.add('sidebar-collapsed');
            if (sidebar) {
                sidebar.classList.add('w-20');
                sidebar.classList.remove('w-64');
                toggleTextVisibility(true);
            }
        } else {
            document.body.classList.remove('sidebar-collapsed');
            if (sidebar) {
                sidebar.classList.remove('w-20');
                sidebar.classList.add('w-64');
                toggleTextVisibility(false);
            }
        }
    }
    
    // Toggle text visibility
    function toggleTextVisibility(hide) {
        const textElements = sidebar.querySelectorAll('.nav-text');
        textElements.forEach(el => {
            if (hide) {
                el.classList.add('hidden');
            } else {
                el.classList.remove('hidden');
            }
        });
    }
    
    // Toggle sidebar
    function toggleSidebar() {
        const willCollapse = !document.body.classList.contains('sidebar-collapsed');
        document.body.classList.toggle('sidebar-collapsed');
        
        if (willCollapse) {
            if (sidebar) {
                sidebar.classList.add('w-20');
                sidebar.classList.remove('w-64');
                toggleTextVisibility(true);
            }
            setCookie('sidebar-collapsed', 'true');
        } else {
            if (sidebar) {
                sidebar.classList.remove('w-20');
                sidebar.classList.add('w-64');
                toggleTextVisibility(false);
            }
            setCookie('sidebar-collapsed', 'false');
        }
    }
    
    // Toggle mobile menu
    function toggleMobileMenu() {
        const isExpanded = mobileMenuButton.getAttribute('aria-expanded') === 'true' || false;
        mobileMenuButton.setAttribute('aria-expanded', !isExpanded);
        
        if (!isExpanded) {
            mobileMenu.classList.remove('hidden');
            setTimeout(() => {
                mobileMenu.classList.remove('opacity-0', 'scale-95');
                mobileMenu.classList.add('opacity-100', 'scale-100');
            }, 10);
        } else {
            mobileMenu.classList.remove('opacity-100', 'scale-100');
            mobileMenu.classList.add('opacity-0', 'scale-95');
            setTimeout(() => {
                mobileMenu.classList.add('hidden');
            }, 150);
        }
    }
    
    // Close mobile menu when clicking outside
    function handleClickOutside(event) {
        if (mobileMenu && !mobileMenu.contains(event.target) && 
            mobileMenuButton && !mobileMenuButton.contains(event.target)) {
            if (!mobileMenu.classList.contains('hidden')) {
                toggleMobileMenu();
            }
        }
    }
    
    // Event listeners
    if (toggleButton) {
        toggleButton.addEventListener('click', toggleSidebar);
    }
    
    if (mobileMenuButton) {
        mobileMenuButton.addEventListener('click', toggleMobileMenu);
    }
    
    // Close mobile menu when clicking on a navigation link
    document.querySelectorAll('#mobileMenu a').forEach(link => {
        link.addEventListener('click', () => {
            if (!mobileMenu.classList.contains('hidden')) {
                toggleMobileMenu();
            }
        });
    });
    
    // Close mobile menu when clicking on overlay
    if (sidebarOverlay) {
        sidebarOverlay.addEventListener('click', () => {
            if (document.body.classList.contains('mobile-sidebar-open')) {
                document.body.classList.remove('mobile-sidebar-open');
                if (sidebarOverlay) {
                    sidebarOverlay.classList.remove('visible');
                }
            }
        });
    }
    
    // Close mobile menu on escape key
    document.addEventListener('keydown', (event) => {
        if (event.key === 'Escape') {
            if (!mobileMenu.classList.contains('hidden')) {
                toggleMobileMenu();
            }
            
            if (document.body.classList.contains('mobile-sidebar-open')) {
                document.body.classList.remove('mobile-sidebar-open');
                if (sidebarOverlay) {
                    sidebarOverlay.classList.remove('visible');
                }
            }
        }
    });
    
    // Initialize
    initSidebar();
    
    // Add click outside handler
    document.addEventListener('click', handleClickOutside);
});
