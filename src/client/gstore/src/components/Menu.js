import React from 'react';

import { Navbar,Nav,NavDropdown, Button } from 'react-bootstrap';
import TokenManager from '../helpers/TokenManager';

const Menu = (props) => {
    const logoutHandler = (event) => {
        TokenManager.clearToken();
        props.logoutCallback();
    } 

    return (
        <Navbar bg="light" expand="lg">
            <Navbar.Brand href="#home">GSTore</Navbar.Brand>
            <Navbar.Toggle aria-controls="basic-navbar-nav" />
            <Navbar.Collapse id="basic-navbar-nav">
                <Nav className="mr-auto">
                <Nav.Link href="/home">Home</Nav.Link>
                <Nav.Link href="/geodatalist">Geodata</Nav.Link>
                {/* <Nav.Link href="/login">Login</Nav.Link> */}
                <NavDropdown title="Admin" id="basic-nav-dropdown">
                    <NavDropdown.Item href="/userlist">User List</NavDropdown.Item>
                    {/* <NavDropdown.Divider />
                    <NavDropdown.Item href="#action/3.4">Separated link</NavDropdown.Item> */}
                </NavDropdown>
                <Button variant="link" size="sm"  onClick={logoutHandler}>
                    Logout
                </Button>
                </Nav>
            </Navbar.Collapse>
        </Navbar>
    );
}


export default Menu;