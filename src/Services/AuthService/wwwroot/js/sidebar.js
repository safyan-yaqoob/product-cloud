document.addEventListener('DOMContentLoaded', function() {
    const sidebar = document.getElementById('sidebar');
    const toggleButton = document.getElementById('toggleSidebar');
    const sidebarOverlay = document.getElementById('sidebarOverlay');
    const mobileMenuButton = document.getElementById('mobileMenuButton');
    const mobileMenu = document.getElementById('mobileMenu');
    
    // Check for saved sidebar state
    const isCollapsed = localStorage.getItem('sidebarCollapsed') === 'true';
    
    // Initialize sidebar state
    function initSidebar() {
        if (isCollapsed) {
            document.body.classList.add('sidebar-collapsed');
            if (sidebar) sidebar.classList.add('w-20');
            if (sidebar) sidebar.classList.remove('w-64');
        } else {
            document.body.classList.remove('sidebar-collapsed');
            if (sidebar) sidebar.classList.remove('w-20');
            if (sidebar) sidebar.classList.add('w-64');
        }
    }
    
    // Toggle sidebar
    function toggleSidebar() {
        const isCollapsed = document.body.classList.toggle('sidebar-collapsed');
        
        if (isCollapsed) {
            if (sidebar) sidebar.classList.add('w-20');
            if (sidebar) sidebar.classList.remove('w-64');
            localStorage.setItem('sidebarCollapsed', 'true');
        } else {
            if (sidebar) sidebar.classList.remove('w-20');
            if (sidebar) sidebar.classList.add('w-64');
            localStorage.setItem('sidebarCollapsed', 'false');
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
