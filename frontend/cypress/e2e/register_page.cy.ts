describe('Register Page', () => {
    beforeEach(() => {
        cy.visit('http://localhost:5173/register'); // Assuming '/register' is the URL of your register page
    });

    // it('allows user to register successfully', () => {
    //     cy.get('#full-name').type('John Doe');
    //     cy.get('#email').type('john@example.com');
    //     cy.get('#password').type('password');
    //     // cy.get('#grade-select').select('Elementary'); // Assuming 'High School' is an available option
    //     cy.get('#school').type('High School of Dreams');
    //
    //     cy.intercept('POST', '/api/register').as('registerRequest');
    //
    //     cy.get('button').contains('Sign Up').click();
    //
    //     cy.wait('@registerRequest').then(() => {
    //         cy.get('.swal2-title').should('contain', 'Success');
    //         // Add more assertions as needed
    //     });
    // });

    it('displays error message if registration fails', () => {
        cy.get('#full-name').type('Jane Doe');
        cy.get('#email').type('jane@example.com');
        cy.get('#password').type('password');
        // cy.get('#grade-select').select('High School');
        cy.get('#school').type('High School of Dreams');

        cy.intercept('POST', '/api/register').as('registerRequest');

        cy.get('button').contains('Sign Up').click();

        // cy.wait('@registerRequest').then((xhr) => {
        //     expect(xhr.response.statusCode).to.eq(500); // Assuming 500 is the status code for failed registration
        //     cy.get('.swal2-title').should('contain', 'Error');
        //     // Add more assertions as needed
        // });
    });

});
