import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import './Navbar.css';
function NavbarOutside() {
    return (
        <Navbar expand="lg" className="navbar-glass">
            <Container>
                <Navbar.Brand href="/">
                    <img
                        src="/images/studysync.png"
                        className="d-inline-block align-top"
                        style={{ maxWidth: '100px' }}
                        alt="Logo"
                    />
                </Navbar.Brand>

                <Navbar.Toggle aria-controls="basic-navbar-nav" />
                <Navbar.Collapse id="basic-navbar-nav">
                    <Nav className="ms-auto">
                        <Nav.Link href="/">Home</Nav.Link>
                        <Nav.Link href="/register">Login | Register</Nav.Link>

                    </Nav>
                </Navbar.Collapse>
            </Container>
        </Navbar>
    );
}

export default NavbarOutside;