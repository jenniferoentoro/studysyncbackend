import React from 'react';
import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import MenuItem from '@mui/material/MenuItem';
import Avatar from '@mui/material/Avatar';
import PersonIcon from '@mui/icons-material/Person';
import Menu from '@mui/material/Menu';
import IconButton from '@mui/material/IconButton';
import LogoutIcon from '@mui/icons-material/Logout';
// import { useHistory } from 'react-router-dom'; // Import useHistory from react-router-dom for navigation
import './Navbar.css';

function NavbarInside() {
    const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
    // const history = useHistory();

    const handleClick = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };
    const handleClose = () => {
        setAnchorEl(null);
    };

    const handleLogout = () => {
        // Perform logout actions, e.g., clear session, redirect to login page
        // history.push('/login'); // Redirect to the login page
    };

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
                        <Nav.Link href="/home">Explore</Nav.Link>
                        <Nav.Link href="/mysources">My Sources</Nav.Link>
                        <IconButton onClick={handleClick}>
                            <Avatar sx={{ width: 32, height: 32 }}>
                                <PersonIcon />
                            </Avatar>
                        </IconButton>
                        <Menu
                            anchorEl={anchorEl}
                            open={Boolean(anchorEl)}
                            onClose={handleClose}
                        >
                            <MenuItem onClick={handleClose}>Profile</MenuItem>
                            <MenuItem onClick={handleLogout}>
                                <LogoutIcon />
                                Logout
                            </MenuItem>
                        </Menu>
                    </Nav>
                </Navbar.Collapse>
            </Container>
        </Navbar>
    );
}

export default NavbarInside;
