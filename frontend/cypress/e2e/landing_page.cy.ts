describe('Landing Page', () => {
    beforeEach(() => {
        cy.visit('http://localhost:5173/'); // Assuming your landing page URL is '/'
    });

    it('should display the Study and Sync headings', () => {
        cy.contains('Study').should('be.visible');
        cy.contains('Sync').should('be.visible');
    });

    it('should display the Sign In and Sign Up buttons', () => {
        cy.contains('Sign In').should('be.visible');
        cy.contains('Sign Up').should('be.visible');
    });

    it('should navigate to login page when Sign In button is clicked', () => {
        cy.contains('Sign In').click();
        cy.url().should('include', '/login');
    });

    it('should navigate to register page when Sign Up button is clicked', () => {
        cy.contains('Sign Up').click();
        cy.url().should('include', '/register');
    });

    it('should display the home page image', () => {
        cy.get('.d-inline-block').should('be.visible');
    });

    // Test responsiveness
    context('Responsive Design', () => {
        beforeEach(() => {
            cy.viewport('iphone-6'); // Set viewport to iPhone 6 size
        });

        it('should display elements properly on mobile view', () => {
            cy.get('.text-5xl').should('have.css', 'font-size', '48px');
            cy.get('.text-xl').should('have.css', 'font-size', '20px');
            cy.get('.px-20').should('have.css', 'padding-left', '80px');
        });
    });
});
